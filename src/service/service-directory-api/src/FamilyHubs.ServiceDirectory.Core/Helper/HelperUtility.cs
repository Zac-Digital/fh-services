using Ardalis.GuardClauses;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using GeoCoordinatePortable;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace FamilyHubs.ServiceDirectory.Core.Helper;

//todo: move code out of this generic helper class
public static class HelperUtility
{
    // this matches the validation we have in the frontend
    private static readonly Regex IsValidUrlRegEx = new(
        @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static async Task<List<TEntity>> GetEntities<TEntity>(
        this ICollection<long> ids,
        DbSet<TEntity> existingEntities)
        where TEntity : EntityBase<long>
    {
        List<TEntity> foundEntities = new();

        foreach (var id in ids)
        {
            var existingEntity = await existingEntities.FindAsync(id);
            if (existingEntity == null)
            {
                //todo: this Ardalis exception doesn't seem to support multiple objects
                //todo: NotFoundException in other commands use either Ardalis or a custom exception
                //todo: the other Ardalis throws seem to have the params the wrong way round
                throw new NotFoundException(id.ToString(), nameof(Location));
            }
            foundEntities.Add(existingEntity);
        }

        return foundEntities;
    }
    
    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool ascending)
    {
        return ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }
    
    public static IOrderedQueryable<TSource> ThenBy<TSource,TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool ascending)
    {
        return ascending ? source.ThenBy(keySelector) : source.ThenByDescending(keySelector);
    }

    public static bool IsValidUrl(string url)
    {
        return IsValidUrlRegEx.IsMatch(url);
    }

}