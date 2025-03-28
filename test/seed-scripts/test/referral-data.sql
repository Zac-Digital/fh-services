BEGIN TRY
    BEGIN TRANSACTION
          
    DECLARE @Today DATETIME2
    SET @Today = GETDATE()

    DELETE FROM [dbo].[Recipients]
                 
    SET IDENTITY_INSERT [dbo].[Recipients] ON

    INSERT INTO [dbo].[Recipients] ([Id], [Name], [Email], [Telephone], [TextPhone], [AddressLine1], [AddressLine2], [TownOrCity], [County], [PostCode], [Created], [CreatedBy], [LastModified], [LastModifiedBy])
    VALUES  (1, N'nHmoPmNKsCPAIx2GLHsqWQ==', N'zj10XhrWmO9F3wiDCmr+9A==', NULL, NULL, NULL, NULL, NULL, NULL, NULL, @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=', @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4='),
            (2, N'hIf3I9vjs3MkQr91vIX+xQ==', NULL, N'dEXc/Uf44P+E7VSeXkOVqA==', NULL, NULL, NULL, NULL, NULL, NULL, @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY='),
            (3, N'ik6etV0f3XZQFvgIGT5FEw==', NULL, NULL, N'PV/56sL4V5tzRaiTGYpFuQ==', NULL, NULL, NULL, NULL, NULL, @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY='),
            (4, N'rtZQdGElqdezLRZSRqvEew==', NULL, NULL, NULL, N'n1rKEANLs1Jw3+O+jBhbnuUJMPRFb5iRHd78Sds9gLM=', N'bB/qq2Iy1Hjfmds6R6S+zx4cp+N4wkewrrPEft+bnMQ=', N'yezZlBv/WKN7yKQ/f5XFbA==', NULL, N'vyT/kH0Q2dt3ORNYsD7NHA==', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=');
    
    SET IDENTITY_INSERT [dbo].[Recipients] OFF
    
    DELETE FROM [dbo].[Organisations]

    INSERT [dbo].[Organisations] ([Id], [Name], [Description], [Created], [CreatedBy], [LastModified], [LastModifiedBy]) VALUES (11, N'Elop Mentoring', N'Elop Mentoring', @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=', @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=')

    DELETE FROM [dbo].[ReferralServices]

    INSERT INTO [dbo].[ReferralServices] ([Id], [Name], [Description], [Url], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [OrganizationId])
    VALUES  (1, N'LGBT+ Asylum support', NULL, NULL, @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=', @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=', 11),
            (3, N'Young People’s Services ‘Youth Out East’', N'In addition to operating direct community frontline services ELOP also delivers second-tier work which includes providing information, training, consultancy and support to statutory and voluntary sector policy makers, managers, service providers and their staff teams.', NULL, @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', 11);
    
    DELETE FROM [dbo].[Statuses]

    INSERT INTO dbo.Statuses ([Id], [Name], [SortOrder], [SecondrySortOrder], [Created], [CreatedBy], [LastModified], [LastModifiedBy])
    VALUES  (1, N'New', 0, 1, @Today, N'a7pEJJRDy9cYFAWGmzBG2A==', @Today, N'a7pEJJRDy9cYFAWGmzBG2A=='),
            (2, N'Opened', 1, 1, @Today, N'a7pEJJRDy9cYFAWGmzBG2A==', @Today, N'a7pEJJRDy9cYFAWGmzBG2A=='),
            (3, N'Accepted', 2, 2, @Today, N'a7pEJJRDy9cYFAWGmzBG2A==', @Today, N'a7pEJJRDy9cYFAWGmzBG2A=='),
            (4, N'Declined', 3, 0, @Today, N'a7pEJJRDy9cYFAWGmzBG2A==', @Today, N'a7pEJJRDy9cYFAWGmzBG2A==');
    
    DELETE FROM [dbo].[UserAccounts]

    INSERT INTO [dbo].[UserAccounts] ([Id], [EmailAddress], [Name], [PhoneNumber], [Team], [Created], [CreatedBy], [LastModified], [LastModifiedBy])
    VALUES  (5, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', N'UIouNuXjCi1ziAuMCQRipw==', NULL, NULL, @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY='),
            (8, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=', N'PVyzprgd3yPHtP8/nTmJOg==', NULL, NULL, @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=', @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=');
    
    DELETE FROM [dbo].[Referrals]
    
    SET IDENTITY_INSERT [dbo].[Referrals] ON

    INSERT INTO [dbo].[Referrals] ([Id], [ReferrerTelephone], [ReasonForSupport], [EngageWithFamily], [ReasonForDecliningSupport], [StatusId], [RecipientId], [UserAccountId], [ReferralServiceId], [Created], [CreatedBy], [LastModified], [LastModifiedBy])
    VALUES  (1, NULL, N'dIesU8T2o92WvXF/c8qV4FZBlTmVJDpQ2YWEKo4DtIrkfIl8DpUU96JWNBDMMfm+', N'jL5QoA+YqdtZJP7j4ovdGQPFkUtdxw8GgYL/foaleR7cf9x02Jn8AsUNT09M9vqrqFuFLHR3GbtFEjprIWOcdk1TfZp0p+xI2rr93vnHmtE=', NULL, 3, 1, 8, 1, @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=', @Today, N'LMwTJJeqI1poDAaS/pC/rRhe0LJr/j7j7maRLCGf+WA='),
            (2, NULL, N'OYoGZ93Ni9BaHvEajdoFyW7wOl6qPvWtYkgRcl4g8745yVdradk93nIckrp8fiZp', N'UKNwu3ctwvAzxTS8yPeF7tJGqAU6oq3f6e9SKPcZ8C8rZqDrAdGWCLSDUm9Zswzx', NULL, 1, 2, 5, 1, @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY='),
            (3, NULL, N'cRC4Tn5ypa6B6bzmaaV4JXf+18EQxqHjTRWZyDCXyMFvS+bpN87tiX7J0pwGSjUD', N'E4+H4GK1k1p8vHMeLXlrv+fXlIyDmSAdhKqvCiJFwuSwBe/KQzqQHwjNY+MImBz3', NULL, 1, 3, 5, 3, @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY='),
            (4, NULL, N'wVDd8Zesbb5VW7X/+y7NdO9bk4EUHeiJ9on8TrDafA4=', N'4cKYaV7AfXvuxvhEPl5deMW2A5KoBnzOswm8+/df+zA=', NULL, 1, 4, 5, 1, @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=');
    
    SET IDENTITY_INSERT [dbo].[Referrals] OFF
    
    DELETE FROM [dbo].[Roles]

    INSERT INTO [dbo].[Roles] ([Id], [Name], [Description], [Created], [CreatedBy], [LastModified], [LastModifiedBy])
    VALUES  (1, N'DfeAdmin', N'DfE Administrator', @Today, N'a7pEJJRDy9cYFAWGmzBG2A==', @Today, N'a7pEJJRDy9cYFAWGmzBG2A=='),
            (2, N'LaManager', N'Local Authority Manager', @Today, N'a7pEJJRDy9cYFAWGmzBG2A==', @Today, N'a7pEJJRDy9cYFAWGmzBG2A=='),
            (3, N'VcsManager', N'VCS Manager', @Today, N'a7pEJJRDy9cYFAWGmzBG2A==', @Today, N'a7pEJJRDy9cYFAWGmzBG2A=='),
            (4, N'LaProfessional', N'Local Authority Professional', @Today, N'a7pEJJRDy9cYFAWGmzBG2A==', @Today, N'a7pEJJRDy9cYFAWGmzBG2A=='),
            (5, N'VcsProfessional', N'VCS Professional', @Today, N'a7pEJJRDy9cYFAWGmzBG2A==', @Today, N'a7pEJJRDy9cYFAWGmzBG2A=='),
            (6, N'LaDualRole', N'Local Authority Dual Role', @Today, N'a7pEJJRDy9cYFAWGmzBG2A==', @Today, N'a7pEJJRDy9cYFAWGmzBG2A=='),
            (7, N'VcsDualRole', N'VCS Dual Role', @Today, N'a7pEJJRDy9cYFAWGmzBG2A==', @Today, N'a7pEJJRDy9cYFAWGmzBG2A==');
    
    DELETE FROM [dbo].[UserAccountRoles]
    
    SET IDENTITY_INSERT [dbo].[UserAccountRoles] ON

    INSERT INTO [dbo].[UserAccountRoles] ([Id], [UserAccountId], [RoleId], [Created], [CreatedBy], [LastModified], [LastModifiedBy])
    VALUES  (1, 8, 6, @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=', @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4='),
            (2, 5, 6, @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=');
    
    SET IDENTITY_INSERT [dbo].[UserAccountRoles] OFF
    
    DELETE FROM [dbo].[ConnectionRequestsSentMetric]
    
    SET IDENTITY_INSERT [dbo].[ConnectionRequestsSentMetric] ON

    INSERT INTO [dbo].[ConnectionRequestsSentMetric] ([Id], [LaOrganisationId], [UserAccountId], [RequestTimestamp], [RequestCorrelationId], [ResponseTimestamp], [HttpResponseCode], [ConnectionRequestId], [ConnectionRequestReferenceCode], [Created], [CreatedBy], [LastModified], [LastModifiedBy], [VcsOrganisationId])
    VALUES  (1, 6, 8, @Today, N'f24e16e5cd60cfb52c2efe15a6f878c7', @Today, 200, 1, N'000001', @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=', @Today, N'sERsnMslwsBWdyACFk2vWLY+TsndIqsblIwJGa1Tlo4=', 11),
            (2, 6, 5, @Today, N'd72fb29dccb170cf23dd71b8c7d41d5a', @Today, 200, 2, N'000002', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', 11),
            (3, 6, 5, @Today, N'46632d5024c4ee355efdfe804755dc36', @Today, 200, 3, N'000003', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', 11),
            (4, 6, 5, @Today, N'20356e51c206c6fb5a545e93d1501ca9', @Today, 200, 4, N'000004', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', @Today, N'I1ELIJzNN8RnjldAy3sEBLppbKxnTR/RXjmWBzJgXWY=', 11);
    
    SET IDENTITY_INSERT [dbo].[ConnectionRequestsSentMetric] OFF
    
    COMMIT TRANSACTION
END TRY
BEGIN CATCH
    SELECT
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() AS ErrorState,
        ERROR_PROCEDURE() AS ErrorProcedure,
        ERROR_LINE() AS ErrorLine,
        ERROR_MESSAGE() AS ErrorMessage
    
    DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;
    
    SELECT
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE(),
        @ErrorMessage = ERROR_MESSAGE()

    ROLLBACK TRANSACTION
    
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
END CATCH
