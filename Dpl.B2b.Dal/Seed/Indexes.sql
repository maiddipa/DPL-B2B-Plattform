-------------------------------------------------- LMS Foreign Keys
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LMS_AVAILABILITY_ClientId' AND object_id = OBJECT_ID('LMS.LMS_AVAILABILITY'))
begin
	CREATE INDEX IX_GENERATED_LMS_AVAILABILITY_ClientId ON [LMS].[LMS_AVAILABILITY]
([ClientId]) WITH(FILLFACTOR = 80)
end
GO

if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LMS_AVAILABILITY_PalletTypeId' AND object_id = OBJECT_ID('LMS.LMS_AVAILABILITY'))
begin
	CREATE INDEX IX_GENERATED_LMS_AVAILABILITY_PalletTypeId ON [LMS].[LMS_AVAILABILITY]
([PalletTypeId]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LMS_AVAILABILITY_QualityId' AND object_id = OBJECT_ID('LMS.LMS_AVAILABILITY'))
begin
	CREATE INDEX IX_GENERATED_LMS_AVAILABILITY_QualityId ON [LMS].[LMS_AVAILABILITY]
([QualityId]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LMS_AVAILABILITY_GeblocktFuer' AND object_id = OBJECT_ID('LMS.LMS_AVAILABILITY'))
begin
	CREATE INDEX IX_GENERATED_LMS_AVAILABILITY_GeblocktFuer ON [LMS].[LMS_AVAILABILITY]
([GeblocktFuer]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LMS_DELIVERY_ClientId' AND object_id = OBJECT_ID('LMS.LMS_DELIVERY'))
begin
	CREATE INDEX IX_GENERATED_LMS_DELIVERY_ClientId ON [LMS].[LMS_DELIVERY]
([ClientId]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LMS_DELIVERY_PalletType' AND object_id = OBJECT_ID('LMS.LMS_DELIVERY'))
begin
	CREATE INDEX IX_GENERATED_LMS_DELIVERY_PalletType ON [LMS].[LMS_DELIVERY]
([PalletType]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LMS_DELIVERY_Quality' AND object_id = OBJECT_ID('LMS.LMS_DELIVERY'))
begin
	CREATE INDEX IX_GENERATED_LMS_DELIVERY_Quality ON [LMS].[LMS_DELIVERY]
([Quality]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LMS_DELIVERY_GeblocktFuer' AND object_id = OBJECT_ID('LMS.LMS_DELIVERY'))
begin
	CREATE INDEX IX_GENERATED_LMS_DELIVERY_GeblocktFuer ON [LMS].[LMS_DELIVERY]
([GeblocktFuer]) WITH(FILLFACTOR = 80)
end


----------------------------------------- LTMS Foreign Keys
GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSAccounts_InvoiceAccount_ID' AND object_id = OBJECT_ID('LTMS.Accounts'))
begin
	CREATE INDEX IX_GENERATED_LTMSAccounts_InvoiceAccount_ID ON[LTMS].[Accounts]
([InvoiceAccount_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSAccounts_Parent_ID' AND object_id = OBJECT_ID('LTMS.Accounts'))
begin
	CREATE INDEX IX_GENERATED_LTMSAccounts_Parent_ID ON[LTMS].[Accounts]
([Parent_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSAccounts_AccountType_ID' AND object_id = OBJECT_ID('LTMS.Accounts'))
begin
	CREATE INDEX IX_GENERATED_LTMSAccounts_AccountType_ID ON[LTMS].[Accounts]
([AccountType_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSAccounts_KeyAccountManager_ID' AND object_id = OBJECT_ID('LTMS.Accounts'))
begin
	CREATE INDEX IX_GENERATED_LTMSAccounts_KeyAccountManager_ID ON[LTMS].[Accounts]
([KeyAccountManager_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSAccounts_ResponsiblePerson_ID' AND object_id = OBJECT_ID('LTMS.Accounts'))
begin
	CREATE INDEX IX_GENERATED_LTMSAccounts_ResponsiblePerson_ID ON[LTMS].[Accounts]
([ResponsiblePerson_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSAccounts_Salesperson_ID' AND object_id = OBJECT_ID('LTMS.Accounts'))
begin
	CREATE INDEX IX_GENERATED_LTMSAccounts_Salesperson_ID ON[LTMS].[Accounts]
([Salesperson_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSBookings_Account_ID' AND object_id = OBJECT_ID('LTMS.Bookings'))
begin
	CREATE INDEX IX_GENERATED_LTMSBookings_Account_ID ON[LTMS].[Bookings]
([Account_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSBookings_Article_ID' AND object_id = OBJECT_ID('LTMS.Bookings'))
begin
	CREATE INDEX IX_GENERATED_LTMSBookings_Article_ID ON[LTMS].[Bookings]
([Article_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSBookings_BookingType_ID' AND object_id = OBJECT_ID('LTMS.Bookings'))
begin
	CREATE INDEX IX_GENERATED_LTMSBookings_BookingType_ID ON[LTMS].[Bookings]
([BookingType_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSBookings_BookingDate' AND object_id = OBJECT_ID('LTMS.Bookings'))
begin
	CREATE INDEX [IX_GENERATED_LTMSBookings_BookingDate] ON[LTMS].[Bookings]
([BookingDate]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSBookings_Quality_ID' AND object_id = OBJECT_ID('LTMS.Bookings'))
begin
	CREATE INDEX IX_GENERATED_LTMSBookings_Quality_ID ON[LTMS].[Bookings]
([Quality_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSBookings_Transaction_ID' AND object_id = OBJECT_ID('LTMS.Bookings'))
begin
	CREATE INDEX IX_GENERATED_LTMSBookings_Transaction_ID ON[LTMS].[Bookings]
([Transaction_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSBookingDependent_ID' AND object_id = OBJECT_ID('LTMS.BookingDependent'))
begin
	CREATE INDEX IX_GENERATED_LTMSBookingDependent_ID ON[LTMS].[BookingDependent]
([ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSConditions_Account_ID' AND object_id = OBJECT_ID('LTMS.Conditions'))
begin
	CREATE INDEX IX_GENERATED_LTMSConditions_Account_ID ON[LTMS].[Conditions]
([Account_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSConditions_Term_ID' AND object_id = OBJECT_ID('LTMS.Conditions'))
begin
	CREATE INDEX IX_GENERATED_LTMSConditions_Term_ID ON[LTMS].[Conditions]
([Term_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSExceptionalOppositeSideAccounts_Account_ID' AND object_id = OBJECT_ID('LTMS.ExceptionalOppositeSideAccounts'))
begin
	CREATE INDEX IX_GENERATED_LTMSExceptionalOppositeSideAccounts_Account_ID ON[LTMS].[ExceptionalOppositeSideAccounts]
([Account_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSExceptionalOppositeSideAccounts_Condition_ID' AND object_id = OBJECT_ID('LTMS.ExceptionalOppositeSideAccounts'))
begin
	CREATE INDEX IX_GENERATED_LTMSExceptionalOppositeSideAccounts_Condition_ID ON[LTMS].[ExceptionalOppositeSideAccounts]
([Condition_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSExceptionalOppositeSideBookingTypes_BookingType_ID' AND object_id = OBJECT_ID('LTMS.ExceptionalOppositeSideBookingTypes'))
begin
	CREATE INDEX IX_GENERATED_LTMSExceptionalOppositeSideBookingTypes_BookingType_ID ON[LTMS].[ExceptionalOppositeSideBookingTypes]
([BookingType_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSExceptionalOppositeSideBookingTypes_Condition_ID' AND object_id = OBJECT_ID('LTMS.ExceptionalOppositeSideBookingTypes'))
begin
	CREATE INDEX IX_GENERATED_LTMSExceptionalOppositeSideBookingTypes_Condition_ID ON[LTMS].[ExceptionalOppositeSideBookingTypes]
([Condition_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSFees_Account_ID' AND object_id = OBJECT_ID('LTMS.Fees'))
begin
	CREATE INDEX IX_GENERATED_LTMSFees_Account_ID ON[LTMS].[Fees]
([Account_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSFees_Booking_ID' AND object_id = OBJECT_ID('LTMS.Fees'))
begin
	CREATE INDEX IX_GENERATED_LTMSFees_Booking_ID ON[LTMS].[Fees]
([Booking_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSFees_Condition_ID' AND object_id = OBJECT_ID('LTMS.Fees'))
begin
	CREATE INDEX IX_GENERATED_LTMSFees_Condition_ID ON[LTMS].[Fees]
([Condition_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSFees_Term_ID' AND object_id = OBJECT_ID('LTMS.Fees'))
begin
	CREATE INDEX IX_GENERATED_LTMSFees_Term_ID ON[LTMS].[Fees]
([Term_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSOnlyValidForOppositeSideAccounts_Account_ID' AND object_id = OBJECT_ID('LTMS.OnlyValidForOppositeSideAccounts'))
begin
	CREATE INDEX IX_GENERATED_LTMSOnlyValidForOppositeSideAccounts_Account_ID ON[LTMS].[OnlyValidForOppositeSideAccounts]
([Account_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSOnlyValidForOppositeSideAccounts_Condition_ID' AND object_id = OBJECT_ID('LTMS.OnlyValidForOppositeSideAccounts'))
begin
	CREATE INDEX IX_GENERATED_LTMSOnlyValidForOppositeSideAccounts_Condition_ID ON[LTMS].[OnlyValidForOppositeSideAccounts]
([Condition_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSOnlyValidForOppositeSideBookingTypes_BookingType_ID' AND object_id = OBJECT_ID('LTMS.OnlyValidForOppositeSideBookingTypes'))
begin
	CREATE INDEX IX_GENERATED_LTMSOnlyValidForOppositeSideBookingTypes_BookingType_ID ON[LTMS].[OnlyValidForOppositeSideBookingTypes]
([BookingType_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSOnlyValidForOppositeSideBookingTypes_Condition_ID' AND object_id = OBJECT_ID('LTMS.OnlyValidForOppositeSideBookingTypes'))
begin
	CREATE INDEX IX_GENERATED_LTMSOnlyValidForOppositeSideBookingTypes_Condition_ID ON[LTMS].[OnlyValidForOppositeSideBookingTypes]
([Condition_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSSwaps_ID' AND object_id = OBJECT_ID('LTMS.Swaps'))
begin
	CREATE INDEX IX_GENERATED_LTMSSwaps_ID ON[LTMS].[Swaps]
([ID]) WITH(FILLFACTOR = 80)
end

GO
--if not exists(SELECT * FROM sys.indexes
--WHERE name= 'IX_GENERATED_LTMSSwaps_Condition_ID' AND object_id = OBJECT_ID('LTMS.Swaps'))
--begin
--	CREATE INDEX IX_GENERATED_LTMSSwaps_Condition_ID ON[LTMS].[Swaps]
--([Condition_ID]) WITH(FILLFACTOR = 80)
--end

--GO
--if not exists(SELECT * FROM sys.indexes
--WHERE name= 'IX_GENERATED_LTMSSwaps_SwapAccount_ID' AND object_id = OBJECT_ID('LTMS.Swaps'))
--begin
--	CREATE INDEX IX_GENERATED_LTMSSwaps_SwapAccount_ID ON[LTMS].[Swaps]
--([SwapAccount_ID]) WITH(FILLFACTOR = 80)
--end

--GO
--if not exists(SELECT * FROM sys.indexes
--WHERE name= 'IX_GENERATED_LTMSSwaps_InBooking_ID' AND object_id = OBJECT_ID('LTMS.Swaps'))
--begin
--	CREATE INDEX IX_GENERATED_LTMSSwaps_InBooking_ID ON[LTMS].[Swaps]
--([InBooking_ID]) WITH(FILLFACTOR = 80)
--end

--GO
--if not exists(SELECT * FROM sys.indexes
--WHERE name= 'IX_GENERATED_LTMSSwaps_OutBooking_ID' AND object_id = OBJECT_ID('LTMS.Swaps'))
--begin
--	CREATE INDEX IX_GENERATED_LTMSSwaps_OutBooking_ID ON[LTMS].[Swaps]
--([OutBooking_ID]) WITH(FILLFACTOR = 80)
--end

--GO
--if not exists(SELECT * FROM sys.indexes
--WHERE name= 'IX_GENERATED_LTMSSwaps_Term_ID' AND object_id = OBJECT_ID('LTMS.Swaps'))
--begin
--	CREATE INDEX IX_GENERATED_LTMSSwaps_Term_ID ON[LTMS].[Swaps]
--([Term_ID]) WITH(FILLFACTOR = 80)
--end

--GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSSwaps_ID' AND object_id = OBJECT_ID('LTMS.Swaps'))
begin
	CREATE INDEX IX_GENERATED_LTMSSwaps_ID ON[LTMS].[Swaps]
([ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSPallet_Article_ID' AND object_id = OBJECT_ID('LTMS.Pallet'))
begin
	CREATE INDEX IX_GENERATED_LTMSPallet_Article_ID ON[LTMS].[Pallet]
([Article_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSPallet_Quality_ID' AND object_id = OBJECT_ID('LTMS.Pallet'))
begin
	CREATE INDEX IX_GENERATED_LTMSPallet_Quality_ID ON[LTMS].[Pallet]
([Quality_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSProcesses_ProcessState_ID' AND object_id = OBJECT_ID('LTMS.Processes'))
begin
	CREATE INDEX IX_GENERATED_LTMSProcesses_ProcessState_ID ON[LTMS].[Processes]
([ProcessState_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSProcesses_ProcessType_ID' AND object_id = OBJECT_ID('LTMS.Processes'))
begin
	CREATE INDEX IX_GENERATED_LTMSProcesses_ProcessType_ID ON[LTMS].[Processes]
([ProcessType_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSReportBookings_Booking_ID' AND object_id = OBJECT_ID('LTMS.ReportBookings'))
begin
	CREATE INDEX IX_GENERATED_LTMSReportBookings_Booking_ID ON[LTMS].[ReportBookings]
([Booking_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSReportBookings_Report_ID' AND object_id = OBJECT_ID('LTMS.ReportBookings'))
begin
	CREATE INDEX IX_GENERATED_LTMSReportBookings_Report_ID ON[LTMS].[ReportBookings]
([Report_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSReports_PrimaryAccount_ID' AND object_id = OBJECT_ID('LTMS.Reports'))
begin
	CREATE INDEX IX_GENERATED_LTMSReports_PrimaryAccount_ID ON[LTMS].[Reports]
([PrimaryAccount_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSReports_ReportState_ID' AND object_id = OBJECT_ID('LTMS.Reports'))
begin
	CREATE INDEX IX_GENERATED_LTMSReports_ReportState_ID ON[LTMS].[Reports]
([ReportState_ID]) WITH(FILLFACTOR = 80)
end

GO

if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSReports_ReportType_ID' AND object_id = OBJECT_ID('LTMS.Reports'))
begin
	CREATE INDEX IX_GENERATED_LTMSReports_ReportType_ID ON[LTMS].[Reports]
([ReportType_ID]) WITH(FILLFACTOR = 80)
end
GO
--if not exists(SELECT * FROM sys.indexes
--WHERE name= 'IX_GENERATED_LTMSReports_Report_ID' AND object_id = OBJECT_ID('LTMS.Reports'))
--begin
--	CREATE INDEX IX_GENERATED_LTMSReports_Report_ID ON[LTMS].[Reports]
--([Report_ID]) WITH(FILLFACTOR = 80)
--end

--GO
--if not exists(SELECT * FROM sys.indexes
--WHERE name= 'IX_GENERATED_LTMSReports_Voucher_ID' AND object_id = OBJECT_ID('LTMS.Reports'))
--begin
--	CREATE INDEX IX_GENERATED_LTMSReports_Voucher_ID ON[LTMS].[Reports]
--([Voucher_ID]) WITH(FILLFACTOR = 80)
--end

--GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSTerms_SwapAccount_ID' AND object_id = OBJECT_ID('LTMS.Terms'))
begin
	CREATE INDEX IX_GENERATED_LTMSTerms_SwapAccount_ID ON[LTMS].[Terms]
([SwapAccount_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSTerms_BookingType_ID' AND object_id = OBJECT_ID('LTMS.Terms'))
begin
	CREATE INDEX IX_GENERATED_LTMSTerms_BookingType_ID ON[LTMS].[Terms]
([BookingType_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSTerms_SwapInBookingType_ID' AND object_id = OBJECT_ID('LTMS.Terms'))
begin
	CREATE INDEX IX_GENERATED_LTMSTerms_SwapInBookingType_ID ON[LTMS].[Terms]
([SwapInBookingType_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSTerms_SwapOutBookingType_ID' AND object_id = OBJECT_ID('LTMS.Terms'))
begin
	CREATE INDEX IX_GENERATED_LTMSTerms_SwapOutBookingType_ID ON[LTMS].[Terms]
([SwapOutBookingType_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSTerms_SwapInPallet_ID' AND object_id = OBJECT_ID('LTMS.Terms'))
begin
	CREATE INDEX IX_GENERATED_LTMSTerms_SwapInPallet_ID ON[LTMS].[Terms]
([SwapInPallet_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSTerms_SwapOutPallet_ID' AND object_id = OBJECT_ID('LTMS.Terms'))
begin
	CREATE INDEX IX_GENERATED_LTMSTerms_SwapOutPallet_ID ON[LTMS].[Terms]
([SwapOutPallet_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSTerms_ChargeWith_ID' AND object_id = OBJECT_ID('LTMS.Terms'))
begin
	CREATE INDEX IX_GENERATED_LTMSTerms_ChargeWith_ID ON[LTMS].[Terms]
([ChargeWith_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSTransactions_Process_ID' AND object_id = OBJECT_ID('LTMS.Transactions'))
begin
	CREATE INDEX IX_GENERATED_LTMSTransactions_Process_ID ON[LTMS].[Transactions]
([Process_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSTransactions_Cancellation_ID' AND object_id = OBJECT_ID('LTMS.Transactions'))
begin
	CREATE INDEX IX_GENERATED_LTMSTransactions_Cancellation_ID ON[LTMS].[Transactions]
([Cancellation_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSTransactions_TransactionState_ID' AND object_id = OBJECT_ID('LTMS.Transactions'))
begin
	CREATE INDEX IX_GENERATED_LTMSTransactions_TransactionState_ID ON[LTMS].[Transactions]
([TransactionState_ID]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LTMSTransactions_TransactionType_ID' AND object_id = OBJECT_ID('LTMS.Transactions'))
begin
	CREATE INDEX IX_GENERATED_LTMSTransactions_TransactionType_ID ON[LTMS].[Transactions]
([TransactionType_ID]) WITH(FILLFACTOR = 80)
end



----------------- App Indexes
-- Live Pooling

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LMS_AVAIL2DELI_DeliveryID_Quantity' AND object_id = OBJECT_ID('LMS.LMS_AVAIL2DELI'))
begin
	CREATE INDEX IX_GENERATED_LMS_AVAIL2DELI_DeliveryID_Quantity ON [LMS].[LMS_AVAIL2DELI]
([DeliveryID]) INCLUDE ([Quantity]) WITH(FILLFACTOR = 80)
end

GO
if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LMS_AVAIL2DELI_AvailabilityID_Quantity' AND object_id = OBJECT_ID('LMS.LMS_AVAIL2DELI'))
begin
	CREATE INDEX IX_GENERATED_LMS_AVAIL2DELI_AvailabilityID_Quantity ON [LMS].[LMS_AVAIL2DELI]
([AvailabilityID]) INCLUDE ([Quantity]) WITH(FILLFACTOR = 80)

end

GO

if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_GENERATED_LMS_DELIVERY_finished_ClientId_Storniert_IstBedarf_FromDate' AND object_id = OBJECT_ID('LMS.LMS_DELIVERY'))
begin
	CREATE NONCLUSTERED INDEX [IX_GENERATED_LMS_DELIVERY_finished_ClientId_Storniert_IstBedarf_FromDate] 
	ON [LMS].[LMS_DELIVERY] ([finished],[ClientId],[Storniert],[IstBedarf],[FromDate])
	INCLUDE([Quality],[UntilDate], [DistributorId],[PalletType],[Geblockt],[GeblocktFuer],[InBearbeitungVon],[InBearbeitungDatumZeit],[RowGuid],[StackHeightMin],[StackHeightMax],[SupportsRearLoading],[SupportsSideLoading],[SupportsJumboVehicles],[BasePalletTypeId],[BaseQualityId],[NeedsBalanceApproval],[BalanceApproved]) 
	WITH(FILLFACTOR = 80)

end

GO

if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_LTMSBookings_AccountId_ArticleId_DeleteTime_RowGuid_ID' AND object_id = OBJECT_ID('LTMS.Bookings'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_LTMSBookings_AccountId_ArticleId_DeleteTime_RowGuid_ID] ON [LTMS].[Bookings]
(
	[Account_ID] ASC,
	[Article_ID] ASC,
	[DeleteTime] ASC,
	[RowGuid] ASC,
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
END
GO


if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_LTMSBookings_AccountId_DeleteTime_RowGuid' AND object_id = OBJECT_ID('LTMS.Bookings'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_LTMSBookings_AccountId_DeleteTime_RowGuid] ON [LTMS].[Bookings]
(
	[Account_ID] ASC,
	[DeleteTime] ASC,
	[RowGuid] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
END

GO

if not exists(SELECT * FROM sys.indexes
WHERE name= 'IX_LTMSVoucherStateId_LegacyId' AND object_id = OBJECT_ID('LTMS.Internals'))
BEGIN
CREATE NONCLUSTERED INDEX [IX_LTMSVoucherStateId_LegacyId]
ON [LTMS].[Internals] ([VoucherState_ID],[Legacy_ID])
END

GO

