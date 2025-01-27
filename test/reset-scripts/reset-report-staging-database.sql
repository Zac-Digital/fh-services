--
-- This is a specific script that will delete all the data that is pulled from various databases, from ADF
--

BEGIN TRY
    BEGIN TRANSACTION
    
    DELETE FROM svcd_db.Organisations
    DELETE FROM svcd_db.Services
    DELETE FROM svcd_db.Contacts
    DELETE FROM svcd_db.AccessibilityForDisabilities
    DELETE FROM svcd_db.CostOptions
    DELETE FROM svcd_db.Eligibilities
    DELETE FROM svcd_db.Fundings
    DELETE FROM svcd_db.Eligibilities
    DELETE FROM svcd_db.Locations
    DELETE FROM svcd_db.Schedules
    DELETE FROM svcd_db.ServiceAreas
    DELETE FROM svcd_db.ServiceAtLocations
    DELETE FROM svcd_db.ServiceDeliveries
    
    DELETE FROM conr_stg.ConnectionRequestsSentFact
    DELETE FROM conr_stg.ConnectionRequestsSentMetric
    
    DELETE FROM svcd_stg.ServiceSearchesFact
    DELETE FROM svcd_stg.ServiceSearchesDim
    DELETE FROM svcd_stg.Organisations
    DELETE FROM svcd_stg.OrganisationsDim
    DELETE FROM svcd_stg.ServiceSearches
    DELETE FROM svcd_stg.ServiceSearchResults
    
    DELETE FROM svcd_mst.Organisations
    DELETE FROM svcd_mst.Services
    DELETE FROM svcd_mst.Locations
    DELETE FROM svcd_mst.ServiceAtLocations
    DELETE FROM svcd_mst.ServiceSearches
    DELETE FROM svcd_mst.ServiceSearchResults

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