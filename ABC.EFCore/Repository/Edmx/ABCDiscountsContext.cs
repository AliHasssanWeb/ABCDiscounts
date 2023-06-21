using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class ABCDiscountsContext : DbContext
    {
        public ABCDiscountsContext()
        {
        }

        public ABCDiscountsContext(DbContextOptions<ABCDiscountsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountClass> AccountClasses { get; set; }
        public virtual DbSet<AccountGroup> AccountGroups { get; set; }
        public virtual DbSet<AccountSubGroup> AccountSubGroups { get; set; }
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<ArticleType> ArticleTypes { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AutherizeOrderLimit> AutherizeOrderLimits { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<BusinessType> BusinessTypes { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }
        public virtual DbSet<CertificateBusinessType> CertificateBusinessTypes { get; set; }
        public virtual DbSet<CertificateExemptionInstruction> CertificateExemptionInstructions { get; set; }
        public virtual DbSet<CertificateIdentification> CertificateIdentifications { get; set; }
        public virtual DbSet<CertificateReasonExemption> CertificateReasonExemptions { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Connection> Connections { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<CreditAdjustment> CreditAdjustments { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerBillingInfo> CustomerBillingInfos { get; set; }
        public virtual DbSet<CustomerClassification> CustomerClassifications { get; set; }
        public virtual DbSet<CustomerDocument> CustomerDocuments { get; set; }
        public virtual DbSet<CustomerInformation> CustomerInformations { get; set; }
        public virtual DbSet<CustomerNote> CustomerNotes { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }
        public virtual DbSet<CustomerPaperWork> CustomerPaperWorks { get; set; }
        public virtual DbSet<CustomerState> CustomerStates { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<DrivingLicenseState> DrivingLicenseStates { get; set; }
        public virtual DbSet<EmpAllowance> EmpAllowances { get; set; }
        public virtual DbSet<EmpAllowanceType> EmpAllowanceTypes { get; set; }
        public virtual DbSet<EmpAllowanceTypeEmp> EmpAllowanceTypeEmps { get; set; }
        public virtual DbSet<EmpAttendance> EmpAttendances { get; set; }
        public virtual DbSet<EmpDeduction> EmpDeductions { get; set; }
        public virtual DbSet<EmpDeductionType> EmpDeductionTypes { get; set; }
        public virtual DbSet<EmpLeave> EmpLeaves { get; set; }
        public virtual DbSet<EmpLeaveType> EmpLeaveTypes { get; set; }
        public virtual DbSet<EmpLoan> EmpLoans { get; set; }
        public virtual DbSet<EmpLoanType> EmpLoanTypes { get; set; }
        public virtual DbSet<EmpPayRoll> EmpPayRolls { get; set; }
        public virtual DbSet<EmpTax> EmpTaxes { get; set; }
        public virtual DbSet<EmpTaxType> EmpTaxTypes { get; set; }
        public virtual DbSet<EmpTaxTypeEmp> EmpTaxTypeEmps { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeAuthorizedRepresentative> EmployeeAuthorizedRepresentatives { get; set; }
        public virtual DbSet<EmployeeContract> EmployeeContracts { get; set; }
        public virtual DbSet<EmployeeDdauthorization> EmployeeDdauthorizations { get; set; }
        public virtual DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public virtual DbSet<EmployeeDocumentType> EmployeeDocumentTypes { get; set; }
        public virtual DbSet<EmployeeLeaveEntitle> EmployeeLeaveEntitles { get; set; }
        public virtual DbSet<EmployeeReverificationAndRehire> EmployeeReverificationAndRehires { get; set; }
        public virtual DbSet<EmployeeSettlement> EmployeeSettlements { get; set; }
        public virtual DbSet<EmployeeWithHoldingTax> EmployeeWithHoldingTaxes { get; set; }
        public virtual DbSet<Employer> Employers { get; set; }
        public virtual DbSet<EmploymentEligibilityVerification> EmploymentEligibilityVerifications { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<FinanceAccount> FinanceAccounts { get; set; }
        public virtual DbSet<Financial> Financials { get; set; }
        public virtual DbSet<ForgetPassword> ForgetPasswords { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<InventoryStock> InventoryStocks { get; set; }
        public virtual DbSet<ItemCategory> ItemCategories { get; set; }
        public virtual DbSet<ItemCostChange> ItemCostChanges { get; set; }
        public virtual DbSet<ItemDocument> ItemDocuments { get; set; }
        public virtual DbSet<ItemSubCategory> ItemSubCategories { get; set; }
        public virtual DbSet<LeaveType> LeaveTypes { get; set; }
        public virtual DbSet<LedgerCategory> LedgerCategories { get; set; }
        public virtual DbSet<MisPick> MisPicks { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<OngoingSaleInvoice> OngoingSaleInvoices { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<PayExpense> PayExpenses { get; set; }
        public virtual DbSet<Payable> Payables { get; set; }
        public virtual DbSet<Paying> Payings { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }
        public virtual DbSet<PaymentTerm> PaymentTerms { get; set; }
        public virtual DbSet<PersonPin> PersonPins { get; set; }
        public virtual DbSet<PosSale> PosSales { get; set; }
        public virtual DbSet<Pricing> Pricings { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<Receivable> Receivables { get; set; }
        public virtual DbSet<Receiving> Receivings { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SaleHistory> SaleHistories { get; set; }
        public virtual DbSet<SalesInvTransaction> SalesInvTransactions { get; set; }
        public virtual DbSet<SalesInvoice> SalesInvoices { get; set; }
        public virtual DbSet<SalesManager> SalesManagers { get; set; }
        public virtual DbSet<Salesman> Salesmen { get; set; }
        public virtual DbSet<SecurityKey> SecurityKeys { get; set; }
        public virtual DbSet<ShipmentPurchase> ShipmentPurchases { get; set; }
        public virtual DbSet<ShiptoReference> ShiptoReferences { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Sttax> Sttaxes { get; set; }
        public virtual DbSet<SubGroup> SubGroups { get; set; }
        public virtual DbSet<Supervisor> Supervisors { get; set; }
        public virtual DbSet<SupervisorCredit> SupervisorCredits { get; set; }
        public virtual DbSet<SupplierCreditPayment> SupplierCreditPayments { get; set; }
        public virtual DbSet<SupplierDocument> SupplierDocuments { get; set; }
        public virtual DbSet<SupplierDocumentType> SupplierDocumentTypes { get; set; }
        public virtual DbSet<SupplierItemNumber> SupplierItemNumbers { get; set; }
        public virtual DbSet<SupplierOtherPayment> SupplierOtherPayments { get; set; }
        public virtual DbSet<SupplierType> SupplierTypes { get; set; }
        public virtual DbSet<SuppliersPayment> SuppliersPayments { get; set; }
        public virtual DbSet<Terminal> Terminals { get; set; }
        public virtual DbSet<TerminalAccess> TerminalAccesses { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<WareHouse> WareHouses { get; set; }
        public virtual DbSet<ZipCode> ZipCodes { get; set; }
        public virtual DbSet<Zone> Zones { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=ABSOLUTESOL\\SQLEXPRESS;Database=ABCDiscounts40j;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountSubGroupId)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Title).IsRequired();

                entity.HasOne(d => d.AccountSubGroup)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AccountSubGroupId)
                    .HasConstraintName("FK_dbo.Accounts_dbo.AccountSubGroups_AccountSubGroupId");
            });

            modelBuilder.Entity<AccountClass>(entity =>
            {
                entity.Property(e => e.AccountClassId)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Title).IsRequired();
            });

            modelBuilder.Entity<AccountGroup>(entity =>
            {
                entity.Property(e => e.AccountGroupId)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.AccountClassId)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Title).IsRequired();

                entity.HasOne(d => d.AccountClass)
                    .WithMany(p => p.AccountGroups)
                    .HasForeignKey(d => d.AccountClassId)
                    .HasConstraintName("FK_dbo.AccountGroups_dbo.AccountClasses_AccountClassId");
            });

            modelBuilder.Entity<AccountSubGroup>(entity =>
            {
                entity.Property(e => e.AccountSubGroupId)
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.AccountGroupId)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Title).IsRequired();

                entity.HasOne(d => d.AccountGroup)
                    .WithMany(p => p.AccountSubGroups)
                    .HasForeignKey(d => d.AccountGroupId)
                    .HasConstraintName("FK_dbo.AccountSubGroups_dbo.AccountGroups_AccountGroupId");
            });

            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.ToTable("Activity_Log");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_Date");

                entity.Property(e => e.Extraone).HasMaxLength(250);

                entity.Property(e => e.Extratwo).HasMaxLength(250);

                entity.Property(e => e.LogTime)
                    .HasMaxLength(50)
                    .HasColumnName("Log_time");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.NewDetails).HasColumnName("New_Details");

                entity.Property(e => e.OldDetails).HasColumnName("Old_Details");

                entity.Property(e => e.OperationName)
                    .HasMaxLength(50)
                    .HasColumnName("Operation_name");

                entity.Property(e => e.PageName)
                    .HasMaxLength(50)
                    .HasColumnName("Page_name");

                entity.Property(e => e.TableName)
                    .HasMaxLength(50)
                    .HasColumnName("Table_Name");

                entity.Property(e => e.UserId).HasColumnName("User_ID");
            });

            modelBuilder.Entity<ArticleType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ArticleTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccessFailedCount).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_date");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Expiry_date");

                entity.Property(e => e.FromScreen).HasMaxLength(250);

                entity.Property(e => e.IsActive).HasColumnName("Is_active");

                entity.Property(e => e.IsCancelled).HasColumnName("Is_cancelled");

                entity.Property(e => e.LastChangePwdDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Last_change_pwd_date");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("Last_login");

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_date");

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("Role_id");

                entity.Property(e => e.StateId).HasColumnName("State_Id");

                entity.Property(e => e.TaxId).HasColumnName("Tax_id");

                entity.Property(e => e.UserPic)
                    .HasColumnType("image")
                    .HasColumnName("User_pic");

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(50)
                    .HasColumnName("Zip_code");
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoleName).HasMaxLength(50);

                entity.Property(e => e.RolesId).HasColumnName("Roles_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<AutherizeOrderLimit>(entity =>
            {
                entity.HasKey(e => e.RequiredId);

                entity.ToTable("AutherizeOrderLimit");

                entity.Property(e => e.RequiredId).HasColumnName("RequiredID");

                entity.Property(e => e.AccessDate).HasColumnType("datetime");

                entity.Property(e => e.AccessTime).HasMaxLength(50);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.ProductCode).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.TicketId).HasMaxLength(50);

                entity.Property(e => e.UploadFile).HasColumnType("image");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<BusinessType>(entity =>
            {
                entity.ToTable("BusinessType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TypeName).HasMaxLength(250);
            });

            modelBuilder.Entity<CartDetail>(entity =>
            {
                entity.HasKey(e => e.CartId);

                entity.ToTable("CartDetail");

                entity.Property(e => e.CartId).HasColumnName("cartID");

                entity.Property(e => e.DeliveredBy).HasMaxLength(50);

                entity.Property(e => e.DeliveredDate)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.ProductCode).HasMaxLength(50);

                entity.Property(e => e.Retail).HasColumnName("retail");

                entity.Property(e => e.TicketId).HasMaxLength(50);

                entity.Property(e => e.Total)
                    .HasMaxLength(50)
                    .HasColumnName("total");
            });

            modelBuilder.Entity<CertificateBusinessType>(entity =>
            {
                entity.HasKey(e => e.Ctbid);

                entity.ToTable("CertificateBusinessType");

                entity.Property(e => e.Ctbid).HasColumnName("CTBID");

                entity.Property(e => e.CertificateId).HasColumnName("CertificateID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            });

            modelBuilder.Entity<CertificateExemptionInstruction>(entity =>
            {
                entity.HasKey(e => e.Ceiid);

                entity.Property(e => e.Ceiid).HasColumnName("CEIID");

                entity.Property(e => e.CountryIssue).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Fein)
                    .HasMaxLength(255)
                    .HasColumnName("FEIN");

                entity.Property(e => e.FeinCountry).HasMaxLength(50);

                entity.Property(e => e.InvoicePurchaseOrderNo).HasColumnName("Invoice_PurchaseOrderNo");

                entity.Property(e => e.PostalAbbreviation)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.PurchaseTaxId)
                    .HasMaxLength(255)
                    .HasColumnName("Purchase_Tax_ID");

                entity.Property(e => e.PurchaserCity).HasMaxLength(255);

                entity.Property(e => e.PurchaserName).HasMaxLength(255);

                entity.Property(e => e.PurchaserState).HasMaxLength(255);

                entity.Property(e => e.PurchaserZipCode).HasMaxLength(50);

                entity.Property(e => e.SellerCity).HasMaxLength(255);

                entity.Property(e => e.SellerName).HasMaxLength(255);

                entity.Property(e => e.SellerState).HasMaxLength(255);

                entity.Property(e => e.SellerZipCode).HasMaxLength(50);

                entity.Property(e => e.Signature).HasColumnType("image");

                entity.Property(e => e.StateIssue).HasMaxLength(255);

                entity.Property(e => e.TermsCondition).HasColumnName("Terms_Condition");
            });

            modelBuilder.Entity<CertificateIdentification>(entity =>
            {
                entity.HasKey(e => e.Ciid);

                entity.ToTable("CertificateIdentification");

                entity.Property(e => e.Ciid).HasColumnName("CIID");

                entity.Property(e => e.CertificateId).HasColumnName("CertificateID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.IdentificationNumber).HasMaxLength(255);

                entity.Property(e => e.State).HasMaxLength(50);
            });

            modelBuilder.Entity<CertificateReasonExemption>(entity =>
            {
                entity.HasKey(e => e.ReasonId);

                entity.ToTable("CertificateReasonExemption");

                entity.Property(e => e.ReasonId).HasColumnName("ReasonID");

                entity.Property(e => e.CertificateId).HasColumnName("CertificateID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Text).HasMaxLength(50);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.ToTable("Color");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<Connection>(entity =>
            {
                entity.HasKey(e => e.SecurityId);

                entity.ToTable("Connection");

                entity.Property(e => e.SecurityId).HasColumnName("Security_ID");

                entity.Property(e => e.AuthToken).HasColumnName("auth_token");

                entity.Property(e => e.Message).HasMaxLength(50);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<CreditAdjustment>(entity =>
            {
                entity.HasKey(e => e.AdjustmentId);

                entity.ToTable("CreditAdjustment");

                entity.Property(e => e.CreditAmount).HasMaxLength(50);

                entity.Property(e => e.CreditDate).HasColumnType("datetime");

                entity.Property(e => e.RemainingAmount).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.CigratteLicenceNumber).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Company).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.CustomerCode).HasMaxLength(50);

                entity.Property(e => e.CustomerState).HasMaxLength(50);

                entity.Property(e => e.CustomerType).HasMaxLength(50);

                entity.Property(e => e.Dea)
                    .HasMaxLength(50)
                    .HasColumnName("DEA");

                entity.Property(e => e.Discount).HasMaxLength(50);

                entity.Property(e => e.DrivingLicense).HasMaxLength(50);

                entity.Property(e => e.DrivingLicenseState).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.FromScreen).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Other).HasMaxLength(50);

                entity.Property(e => e.PasswordHash).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.PostalCode).HasMaxLength(50);

                entity.Property(e => e.ProfileImage).HasColumnType("image");

                entity.Property(e => e.Provider).HasMaxLength(50);

                entity.Property(e => e.RegistrationType).HasMaxLength(50);

                entity.Property(e => e.StateResaleTaxId)
                    .HasMaxLength(50)
                    .HasColumnName("StateResaleTaxID");

                entity.Property(e => e.Street).HasMaxLength(50);

                entity.Property(e => e.TaxIdfein)
                    .HasMaxLength(50)
                    .HasColumnName("TaxIDFEIN");

                entity.Property(e => e.TobaccoLicenseNumber).HasMaxLength(50);

                entity.Property(e => e.Vendor).HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerBillingInfo>(entity =>
            {
                entity.ToTable("CustomerBillingInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdditionalInvoiceCharge).HasMaxLength(250);

                entity.Property(e => e.AdditionalInvoiceDiscount).HasMaxLength(250);

                entity.Property(e => e.CashBackBalance).HasMaxLength(250);

                entity.Property(e => e.CreditLimit).HasMaxLength(250);

                entity.Property(e => e.CustomerClassificationId).HasColumnName("CustomerClassificationID");

                entity.Property(e => e.CustomerCode).HasMaxLength(250);

                entity.Property(e => e.CustomerInformationId).HasColumnName("CustomerInformationID");

                entity.Property(e => e.PaymentTerms).HasMaxLength(250);

                entity.Property(e => e.Pricing).HasMaxLength(250);

                entity.Property(e => e.PricingId).HasColumnName("PricingID");

                entity.Property(e => e.RetailPlus).HasMaxLength(250);

                entity.Property(e => e.RetailPlusPercentage).HasMaxLength(250);

                entity.Property(e => e.ScheduleMessageFromDate).HasColumnType("datetime");

                entity.Property(e => e.ScheduleMessageToDate).HasColumnType("datetime");

                entity.Property(e => e.ThirdPartyCheckCharge).HasMaxLength(250);
            });

            modelBuilder.Entity<CustomerClassification>(entity =>
            {
                entity.ToTable("CustomerClassification");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BarCode).HasMaxLength(250);

                entity.Property(e => e.BarCodeId).HasColumnName("BarCodeID");

                entity.Property(e => e.BusinessType).HasMaxLength(250);

                entity.Property(e => e.BusinessTypeId).HasColumnName("BusinessTypeID");

                entity.Property(e => e.CustomerCode).HasMaxLength(250);

                entity.Property(e => e.CustomerInfoId).HasColumnName("CustomerInfoID");

                entity.Property(e => e.DefaultInvoiceCopies).HasMaxLength(250);

                entity.Property(e => e.DriverId).HasColumnName("DriverID");

                entity.Property(e => e.DriverName).HasMaxLength(250);

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.GroupName).HasMaxLength(250);

                entity.Property(e => e.OtherCustomerReference).HasMaxLength(250);

                entity.Property(e => e.RouteDeliveryDay).HasMaxLength(250);

                entity.Property(e => e.RouteId).HasColumnName("RouteID");

                entity.Property(e => e.RouteName).HasMaxLength(250);

                entity.Property(e => e.Salesman).HasMaxLength(250);

                entity.Property(e => e.SalesmanId).HasColumnName("SalesmanID");

                entity.Property(e => e.ShippedVia).HasMaxLength(250);

                entity.Property(e => e.ShippedViaId).HasColumnName("ShippedViaID");

                entity.Property(e => e.ShiptoReference).HasMaxLength(250);

                entity.Property(e => e.ShiptoReferenceId).HasColumnName("ShiptoReferenceID");

                entity.Property(e => e.SubGroupId).HasColumnName("SubGroupID");

                entity.Property(e => e.SubGroupName).HasMaxLength(250);

                entity.Property(e => e.Zone).HasMaxLength(250);

                entity.Property(e => e.ZoneId).HasColumnName("ZoneID");
            });

            modelBuilder.Entity<CustomerDocument>(entity =>
            {
                entity.HasKey(e => e.DocumentId);

                entity.Property(e => e.DocumentId).HasColumnName("DocumentID");

                entity.Property(e => e.DocumentName).HasMaxLength(50);

                entity.Property(e => e.DocumentType).HasMaxLength(50);

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.UploadDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CustomerInformation>(entity =>
            {
                entity.ToTable("CustomerInformation");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Balance).HasMaxLength(250);

                entity.Property(e => e.BusinessAddress).HasMaxLength(250);

                entity.Property(e => e.Cell).HasMaxLength(250);

                entity.Property(e => e.CigaretteLicenseNumber).HasMaxLength(250);

                entity.Property(e => e.City)
                    .HasMaxLength(250)
                    .HasColumnName("CIty");

                entity.Property(e => e.Country).HasMaxLength(250);

                entity.Property(e => e.CustomerState).HasMaxLength(50);

                entity.Property(e => e.CustomerType).HasMaxLength(250);

                entity.Property(e => e.CustomerTypeId).HasColumnName("CustomerTypeID");

                entity.Property(e => e.Dea)
                    .HasMaxLength(250)
                    .HasColumnName("DEA");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("DOB");

                entity.Property(e => e.DrivingLicenseNumber).HasMaxLength(250);

                entity.Property(e => e.DrivingLicenseState).HasMaxLength(250);

                entity.Property(e => e.DrivingLicenseStateId).HasColumnName("DrivingLicenseStateID");

                entity.Property(e => e.Fax).HasMaxLength(250);

                entity.Property(e => e.FirstName).HasMaxLength(250);

                entity.Property(e => e.FromScreen).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(250);

                entity.Property(e => e.LastName).HasMaxLength(250);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.Other).HasMaxLength(250);

                entity.Property(e => e.OwnerAddress).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(250);

                entity.Property(e => e.PostalCode).HasMaxLength(50);

                entity.Property(e => e.Provider).HasMaxLength(250);

                entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

                entity.Property(e => e.RegistrationType).HasMaxLength(50);

                entity.Property(e => e.Ssn)
                    .HasMaxLength(250)
                    .HasColumnName("SSN");

                entity.Property(e => e.State).HasMaxLength(250);

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.StateIdnumber)
                    .HasMaxLength(250)
                    .HasColumnName("StateIDNumber");

                entity.Property(e => e.StateResaleTaxId).HasMaxLength(50);

                entity.Property(e => e.Street).HasMaxLength(250);

                entity.Property(e => e.TaxIdfein)
                    .HasMaxLength(250)
                    .HasColumnName("TaxIDFEIN");

                entity.Property(e => e.TobaccoLicenseNumber).HasMaxLength(250);

                entity.Property(e => e.VehicleNumber).HasMaxLength(250);

                entity.Property(e => e.Vendor).HasMaxLength(250);

                entity.Property(e => e.Website).HasMaxLength(250);

                entity.Property(e => e.Zip).HasMaxLength(250);
            });

            modelBuilder.Entity<CustomerNote>(entity =>
            {
                entity.HasKey(e => e.NoteId);

                entity.Property(e => e.NoteId).HasColumnName("NoteID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.CustomerName).HasMaxLength(520);

                entity.Property(e => e.CustomerNote1).HasColumnName("CustomerNote");

                entity.Property(e => e.NoteDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("CustomerOrder");

                entity.Property(e => e.AdminActionBy).HasMaxLength(50);

                entity.Property(e => e.AdminActionDate).HasColumnType("datetime");

                entity.Property(e => e.AdminActionTime).HasMaxLength(50);

                entity.Property(e => e.BillingAddress).HasMaxLength(200);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.CustomerCode).HasMaxLength(50);

                entity.Property(e => e.CustomerName).HasMaxLength(100);

                entity.Property(e => e.DeliveredBy).HasMaxLength(50);

                entity.Property(e => e.DeliveredDate).HasColumnType("datetime");

                entity.Property(e => e.DeliveredTime).HasMaxLength(50);

                entity.Property(e => e.DueDate).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.ExpectedDate).HasColumnType("datetime");

                entity.Property(e => e.FinalDate).HasColumnType("datetime");

                entity.Property(e => e.InvoicedBy).HasMaxLength(50);

                entity.Property(e => e.InvoicedDate).HasColumnType("datetime");

                entity.Property(e => e.InvoicedTime).HasMaxLength(50);

                entity.Property(e => e.OrderAmount).HasMaxLength(100);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Price).HasMaxLength(50);

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.PulledBy).HasMaxLength(50);

                entity.Property(e => e.PulledDate).HasColumnType("datetime");

                entity.Property(e => e.PulledTime).HasMaxLength(50);

                entity.Property(e => e.Quantity).HasMaxLength(50);

                entity.Property(e => e.TaxAmount).HasMaxLength(100);

                entity.Property(e => e.TerminalNumber).HasMaxLength(50);

                entity.Property(e => e.TicketId).HasMaxLength(50);

                entity.Property(e => e.Zipcode).HasMaxLength(50);
            });

            modelBuilder.Entity<CustomerPaperWork>(entity =>
            {
                entity.HasKey(e => e.PaperId)
                    .HasName("PK_Customer PaperWork");

                entity.ToTable("CustomerPaperWork");

                entity.Property(e => e.PaperId).HasColumnName("PaperID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DrivingLicenseId)
                    .HasColumnType("image")
                    .HasColumnName("DrivingLicenseID");

                entity.Property(e => e.DrivingLicenseIdpath).HasColumnName("DrivingLicenseIDPath");

                entity.Property(e => e.FederalForm).HasColumnType("image");

                entity.Property(e => e.FromScreen).HasMaxLength(50);

                entity.Property(e => e.SalesTaxId)
                    .HasColumnType("image")
                    .HasColumnName("SalesTaxID");

                entity.Property(e => e.SalesTaxIdpath).HasColumnName("SalesTaxIDPath");

                entity.Property(e => e.UploadedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<CustomerState>(entity =>
            {
                entity.ToTable("CustomerState");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.StateName).HasMaxLength(250);
            });

            modelBuilder.Entity<CustomerType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TypeName).HasMaxLength(250);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.ToTable("Driver");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(250);

                entity.Property(e => e.DrivingLicenseNumber).HasMaxLength(250);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Mobile).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(250);

                entity.Property(e => e.RegisteredOn).HasMaxLength(250);

                entity.Property(e => e.VehicleNumber).HasMaxLength(250);
            });

            modelBuilder.Entity<DrivingLicenseState>(entity =>
            {
                entity.ToTable("DrivingLicenseState");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<EmpAllowance>(entity =>
            {
                entity.ToTable("EmpAllowance");

                entity.Property(e => e.EmpAllowanceId).HasColumnName("EmpAllowanceID");

                entity.Property(e => e.AllowanceTypeId).HasColumnName("AllowanceTypeID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            });

            modelBuilder.Entity<EmpAllowanceType>(entity =>
            {
                entity.ToTable("EmpAllowanceType");

                entity.Property(e => e.EmpAllowanceTypeId).HasColumnName("EmpAllowanceTypeID");
            });

            modelBuilder.Entity<EmpAllowanceTypeEmp>(entity =>
            {
                entity.ToTable("EmpAllowanceTypeEmp");

                entity.Property(e => e.EmpAllowanceTypeEmpId).HasColumnName("EmpAllowanceTypeEmpID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmpAllowanceTypeId).HasColumnName("EmpAllowanceTypeID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            });

            modelBuilder.Entity<EmpAttendance>(entity =>
            {
                entity.ToTable("EmpAttendance");

                entity.Property(e => e.EmpAttendanceId).HasColumnName("EmpAttendanceID");

                entity.Property(e => e.AttendanceDate).HasColumnType("date");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Late).HasMaxLength(50);

                entity.Property(e => e.OverTime).HasMaxLength(50);

                entity.Property(e => e.TimeIn).HasMaxLength(50);

                entity.Property(e => e.TimeOut).HasMaxLength(50);
            });

            modelBuilder.Entity<EmpDeduction>(entity =>
            {
                entity.ToTable("EmpDeduction");

                entity.Property(e => e.EmpDeductionId).HasColumnName("EmpDeductionID");

                entity.Property(e => e.ClaimDate).HasColumnType("datetime");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmpDeductionTypeId).HasColumnName("EmpDeductionTypeID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.IsClaim).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<EmpDeductionType>(entity =>
            {
                entity.ToTable("EmpDeductionType");

                entity.Property(e => e.EmpDeductionTypeId).HasColumnName("EmpDeductionTypeID");
            });

            modelBuilder.Entity<EmpLeave>(entity =>
            {
                entity.ToTable("EmpLeave");

                entity.Property(e => e.EmpLeaveId).HasColumnName("EmpLeaveID");

                entity.Property(e => e.EmpLeaveTypeId).HasColumnName("EmpLeaveTypeID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.FromDate).HasColumnType("date");

                entity.Property(e => e.ToDate).HasColumnType("date");
            });

            modelBuilder.Entity<EmpLeaveType>(entity =>
            {
                entity.ToTable("EmpLeaveType");

                entity.Property(e => e.EmpLeaveTypeId).HasColumnName("EmpLeaveTypeID");
            });

            modelBuilder.Entity<EmpLoan>(entity =>
            {
                entity.ToTable("EmpLoan");

                entity.Property(e => e.EmpLoanId).HasColumnName("EmpLoanID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmpLoanTypeId).HasColumnName("EmpLoanTypeID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.IsPaid).HasDefaultValueSql("((0))");

                entity.Property(e => e.PaidDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmpLoanType>(entity =>
            {
                entity.ToTable("EmpLoanType");

                entity.Property(e => e.EmpLoanTypeId).HasColumnName("EmpLoanTypeID");
            });

            modelBuilder.Entity<EmpPayRoll>(entity =>
            {
                entity.ToTable("EmpPayRoll");

                entity.Property(e => e.EmpPayRollId).HasColumnName("EmpPayRollID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            });

            modelBuilder.Entity<EmpTax>(entity =>
            {
                entity.ToTable("EmpTax");

                entity.Property(e => e.EmpTaxId).HasColumnName("EmpTaxID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmpTaxTypeId).HasColumnName("EmpTaxTypeID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            });

            modelBuilder.Entity<EmpTaxType>(entity =>
            {
                entity.ToTable("EmpTaxType");

                entity.Property(e => e.EmpTaxTypeId).HasColumnName("EmpTaxTypeID");

                entity.Property(e => e.SalaryRange).IsRequired();

                entity.Property(e => e.TaxType).IsRequired();
            });

            modelBuilder.Entity<EmpTaxTypeEmp>(entity =>
            {
                entity.ToTable("EmpTaxTypeEmp");

                entity.Property(e => e.EmpTaxTypeEmpId).HasColumnName("EmpTaxTypeEmpID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmpTaxTypeId).HasColumnName("EmpTaxTypeID");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.DateofHire).HasColumnType("datetime");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("DOB");

                entity.Property(e => e.DrivingLisence).HasMaxLength(250);

                entity.Property(e => e.Ein)
                    .HasMaxLength(50)
                    .HasColumnName("EIN");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.EmployeeCode).HasMaxLength(250);

                entity.Property(e => e.EmployeeOfficeCode).HasMaxLength(50);

                entity.Property(e => e.EmployeeZipCode).HasMaxLength(250);

                entity.Property(e => e.EmployerPhone).HasMaxLength(50);

                entity.Property(e => e.EmployerZipCode).HasMaxLength(250);

                entity.Property(e => e.Expirationdate).HasColumnType("datetime");

                entity.Property(e => e.Extention).HasMaxLength(250);

                entity.Property(e => e.FederalEmployeeId)
                    .HasMaxLength(250)
                    .HasColumnName("FederalEmployeeID");

                entity.Property(e => e.MartialStatus).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(250);

                entity.Property(e => e.NoofChildren).HasMaxLength(50);

                entity.Property(e => e.PayrolAddress).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(250);

                entity.Property(e => e.ProfileImage).HasColumnType("image");

                entity.Property(e => e.Ssn).HasColumnName("SSN");

                entity.Property(e => e.State).HasMaxLength(250);
            });

            modelBuilder.Entity<EmployeeAuthorizedRepresentative>(entity =>
            {
                entity.HasKey(e => e.EmpAuthRepId);

                entity.ToTable("EmployeeAuthorizedRepresentative");

                entity.Property(e => e.EmpAuthRepId).HasColumnName("EmpAuthRepID");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DocumentNumber).HasMaxLength(50);

                entity.Property(e => e.DocumentTitle).HasMaxLength(50);

                entity.Property(e => e.EmpAuthDocumentNumber).HasMaxLength(50);

                entity.Property(e => e.EmpAuthDocumentTitle).HasMaxLength(50);

                entity.Property(e => e.EmpAuthExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.EmpAuthIssuingAuthority).HasMaxLength(50);

                entity.Property(e => e.EmpTitle).HasMaxLength(50);

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.FirstDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.IssuingAuthority).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.OrgName).HasMaxLength(50);

                entity.Property(e => e.Signature).HasMaxLength(50);

                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.ZipCode).HasMaxLength(50);
            });

            modelBuilder.Entity<EmployeeContract>(entity =>
            {
                entity.HasKey(e => e.ContractId);

                entity.ToTable("EmployeeContract");

                entity.Property(e => e.ContractId).HasColumnName("ContractID");

                entity.Property(e => e.ContractDate).HasColumnType("datetime");

                entity.Property(e => e.ContractDocument).HasColumnType("image");

                entity.Property(e => e.ContractName).HasMaxLength(50);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EmployeeNumber).HasMaxLength(50);

                entity.Property(e => e.Iban).HasColumnName("IBAN");

                entity.Property(e => e.JoiningDate).HasColumnType("datetime");

                entity.Property(e => e.ProbationEdate)
                    .HasColumnType("datetime")
                    .HasColumnName("ProbationEDate");

                entity.Property(e => e.ProbationSalary).HasMaxLength(50);

                entity.Property(e => e.ProbationSdate)
                    .HasColumnType("datetime")
                    .HasColumnName("ProbationSDate");

                entity.Property(e => e.Salary).HasMaxLength(50);

                entity.Property(e => e.WorkingTimeIn).HasMaxLength(50);

                entity.Property(e => e.WorkingTimeOut).HasMaxLength(50);
            });

            modelBuilder.Entity<EmployeeDdauthorization>(entity =>
            {
                entity.HasKey(e => e.EmpDdaid);

                entity.ToTable("EmployeeDDAuthorization");

                entity.Property(e => e.EmpDdaid).HasColumnName("EmpDDAID");

                entity.Property(e => e.AccTypeOne).HasMaxLength(50);

                entity.Property(e => e.AccTypeThree).HasMaxLength(50);

                entity.Property(e => e.AccTypeTwo).HasMaxLength(50);

                entity.Property(e => e.Bank).HasMaxLength(50);

                entity.Property(e => e.BankAccNumberOne).HasMaxLength(50);

                entity.Property(e => e.BankAccNumberThree).HasMaxLength(50);

                entity.Property(e => e.BankAccNumberTwo).HasMaxLength(50);

                entity.Property(e => e.BankAmountOne).HasMaxLength(50);

                entity.Property(e => e.BankAmountThree).HasMaxLength(50);

                entity.Property(e => e.BankAmountTwo).HasMaxLength(50);

                entity.Property(e => e.BankName).HasMaxLength(50);

                entity.Property(e => e.BankNameOne).HasMaxLength(50);

                entity.Property(e => e.BankNameThree).HasMaxLength(50);

                entity.Property(e => e.BankNameTwo).HasMaxLength(50);

                entity.Property(e => e.BankRoutingOne).HasMaxLength(50);

                entity.Property(e => e.BankRoutingThree).HasMaxLength(50);

                entity.Property(e => e.BankRoutingTwo).HasMaxLength(50);

                entity.Property(e => e.CheckAmount).HasMaxLength(50);

                entity.Property(e => e.CheckDate).HasColumnType("datetime");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DepositName).HasMaxLength(50);

                entity.Property(e => e.EmployerName).HasMaxLength(50);

                entity.Property(e => e.PayTo).HasMaxLength(50);

                entity.Property(e => e.PctOne).HasMaxLength(50);

                entity.Property(e => e.PctThree).HasMaxLength(50);

                entity.Property(e => e.PctTwo).HasMaxLength(50);

                entity.Property(e => e.PhoneOne).HasMaxLength(50);

                entity.Property(e => e.PhoneThree).HasMaxLength(50);

                entity.Property(e => e.PhoneTwo).HasMaxLength(50);

                entity.Property(e => e.Signature).HasMaxLength(50);

                entity.Property(e => e.Total).HasMaxLength(50);
            });

            modelBuilder.Entity<EmployeeDocument>(entity =>
            {
                entity.HasKey(e => e.DocId);

                entity.Property(e => e.DocId).HasColumnName("DocID");

                entity.Property(e => e.DocTypeId).HasColumnName("DocTypeID");

                entity.Property(e => e.Document).HasColumnType("image");

                entity.Property(e => e.DocumentTypeName).HasMaxLength(50);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EmployeeName).HasMaxLength(50);

                entity.Property(e => e.EmployeeNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<EmployeeDocumentType>(entity =>
            {
                entity.HasKey(e => e.DocTypeId);

                entity.ToTable("EmployeeDocumentType");

                entity.Property(e => e.DocTypeId).HasColumnName("DocTypeID");

                entity.Property(e => e.TypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<EmployeeLeaveEntitle>(entity =>
            {
                entity.HasKey(e => e.EleaveId);

                entity.ToTable("EmployeeLeaveEntitle");

                entity.Property(e => e.EleaveId).HasColumnName("ELeaveID");

                entity.Property(e => e.ApprovedLeave).HasMaxLength(50);

                entity.Property(e => e.AvailableLeave).HasMaxLength(50);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EmployeeName).HasMaxLength(50);

                entity.Property(e => e.EmployeeNo).HasMaxLength(50);

                entity.Property(e => e.LeaveTypeName).HasMaxLength(50);

                entity.Property(e => e.NoofLeaves).HasMaxLength(50);

                entity.Property(e => e.PendingLeave).HasMaxLength(50);

                entity.Property(e => e.RejectedLeave).HasMaxLength(50);
            });

            modelBuilder.Entity<EmployeeReverificationAndRehire>(entity =>
            {
                entity.HasKey(e => e.EmpReverificationId)
                    .HasName("PK_EmployeeReVerification");

                entity.Property(e => e.EmpReverificationId).HasColumnName("EmpReverificationID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DateofRehire).HasColumnType("datetime");

                entity.Property(e => e.DocumentNumber).HasMaxLength(50);

                entity.Property(e => e.DocumentTitle).HasMaxLength(50);

                entity.Property(e => e.EmpName).HasMaxLength(50);

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.NewName).HasMaxLength(50);

                entity.Property(e => e.Signature).HasMaxLength(50);
            });

            modelBuilder.Entity<EmployeeSettlement>(entity =>
            {
                entity.HasKey(e => e.SettlementId);

                entity.ToTable("EmployeeSettlement");

                entity.Property(e => e.SettlementId).HasColumnName("SettlementID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EmployeeName).HasMaxLength(255);

                entity.Property(e => e.PendingLeave).HasMaxLength(50);

                entity.Property(e => e.TerminationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmployeeWithHoldingTax>(entity =>
            {
                entity.HasKey(e => e.Eacid);

                entity.ToTable("EmployeeWithHoldingTax");

                entity.Property(e => e.Eacid).HasColumnName("EACID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Ein)
                    .HasMaxLength(50)
                    .HasColumnName("EIN");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EmployeeOfficeCode).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.MarriedStatus).HasMaxLength(50);

                entity.Property(e => e.NameDiffSsn).HasColumnName("NameDiffSSN");

                entity.Property(e => e.NoofAllowances).HasMaxLength(255);

                entity.Property(e => e.Ssn).HasColumnName("SSN");

                entity.Property(e => e.State).HasMaxLength(255);
            });

            modelBuilder.Entity<Employer>(entity =>
            {
                entity.ToTable("Employer");

                entity.Property(e => e.Branding).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CompanyHeader).HasColumnType("image");

                entity.Property(e => e.DatabaseYear).HasMaxLength(50);

                entity.Property(e => e.DebitCharge).HasMaxLength(50);

                entity.Property(e => e.DefaultCreditCharge).HasMaxLength(50);

                entity.Property(e => e.DefaultMap).HasMaxLength(50);

                entity.Property(e => e.DefaultSalesTax).HasMaxLength(50);

                entity.Property(e => e.EmployerEmail).HasMaxLength(50);

                entity.Property(e => e.EmployerName).HasMaxLength(50);

                entity.Property(e => e.EmployerPhone).HasMaxLength(50);

                entity.Property(e => e.EmployerState).HasMaxLength(50);

                entity.Property(e => e.EmployerZipCode).HasMaxLength(50);

                entity.Property(e => e.Extention).HasMaxLength(50);

                entity.Property(e => e.Fax).HasMaxLength(50);

                entity.Property(e => e.FedTaxId)
                    .HasMaxLength(50)
                    .HasColumnName("FedTaxID");

                entity.Property(e => e.LogoImage).HasColumnType("image");

                entity.Property(e => e.PrintCopies).HasMaxLength(50);

                entity.Property(e => e.StateTaxId)
                    .HasMaxLength(50)
                    .HasColumnName("StateTaxID");

                entity.Property(e => e.Suite).HasMaxLength(50);

                entity.Property(e => e.Zip).HasMaxLength(50);
            });

            modelBuilder.Entity<EmploymentEligibilityVerification>(entity =>
            {
                entity.HasKey(e => e.Eevid);

                entity.ToTable("EmploymentEligibilityVerification");

                entity.Property(e => e.AdmissionNumber).HasMaxLength(50);

                entity.Property(e => e.AllienAuthorizedNumber).HasMaxLength(50);

                entity.Property(e => e.AllienRegNumber).HasMaxLength(50);

                entity.Property(e => e.AptNumber).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CountryIssuance).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Dob).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.EmployeeSignature).HasMaxLength(50);

                entity.Property(e => e.Expirationdate).HasColumnType("datetime");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.ForeignPassportNumber).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.LawfulNumber).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.OtherNames).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.PrepareCity).HasMaxLength(50);

                entity.Property(e => e.PrepareDate).HasColumnType("datetime");

                entity.Property(e => e.PrepareFirstName).HasMaxLength(50);

                entity.Property(e => e.PrepareLastName).HasMaxLength(50);

                entity.Property(e => e.PrepareState).HasMaxLength(50);

                entity.Property(e => e.PrepareZipCode).HasMaxLength(50);

                entity.Property(e => e.SignatureOfTransferer).HasMaxLength(50);

                entity.Property(e => e.Ssn).HasMaxLength(50);

                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.ZipCode).HasMaxLength(50);
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("Faq");
            });

            modelBuilder.Entity<FinanceAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountType).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Financial>(entity =>
            {
                entity.ToTable("Financial");

                entity.Property(e => e.FinancialId).HasColumnName("FinancialID");

                entity.Property(e => e.AddToCost).HasMaxLength(250);

                entity.Property(e => e.Adjustment).HasMaxLength(250);

                entity.Property(e => e.AutoSetSrp).HasColumnName("AutoSetSRP");

                entity.Property(e => e.Cost).HasMaxLength(250);

                entity.Property(e => e.MsgPromotion).HasMaxLength(250);

                entity.Property(e => e.OutOfStateCost).HasMaxLength(250);

                entity.Property(e => e.OutOfStateRetail).HasMaxLength(250);

                entity.Property(e => e.Price).HasMaxLength(250);

                entity.Property(e => e.Profit).HasMaxLength(250);

                entity.Property(e => e.Quantity).HasMaxLength(250);

                entity.Property(e => e.QuantityInStock).HasMaxLength(250);

                entity.Property(e => e.QuantityPrice).HasMaxLength(250);

                entity.Property(e => e.Retail).HasMaxLength(250);

                entity.Property(e => e.St)
                    .HasMaxLength(250)
                    .HasColumnName("ST");

                entity.Property(e => e.SuggestedRetailPrice).HasMaxLength(250);

                entity.Property(e => e.Tax).HasMaxLength(250);

                entity.Property(e => e.UnitCharge).HasMaxLength(250);
            });

            modelBuilder.Entity<ForgetPassword>(entity =>
            {
                entity.ToTable("ForgetPassword");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FromScreen).HasMaxLength(250);

                entity.Property(e => e.Otp).HasColumnName("OTP");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<InventoryStock>(entity =>
            {
                entity.HasKey(e => e.StockId);

                entity.ToTable("InventoryStock");

                entity.Property(e => e.StockId).HasColumnName("StockID");

                entity.Property(e => e.ItemCode).HasMaxLength(50);

                entity.Property(e => e.ItemName).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Quantity).HasMaxLength(50);

                entity.Property(e => e.RemainingDays).HasMaxLength(50);

                entity.Property(e => e.Sku).HasColumnName("SKU");
            });

            modelBuilder.Entity<ItemCategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryImage).HasColumnType("image");

                entity.Property(e => e.Color).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ItemCostChange>(entity =>
            {
                entity.ToTable("ItemCostChange");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_Date");

                entity.Property(e => e.CretedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Creted_By");

                entity.Property(e => e.ItemName).HasMaxLength(50);

                entity.Property(e => e.ItemNumber)
                    .HasMaxLength(50)
                    .HasColumnName("Item_Number");

                entity.Property(e => e.NewPrice)
                    .HasMaxLength(50)
                    .HasColumnName("New_Price");

                entity.Property(e => e.OldPrice)
                    .HasMaxLength(50)
                    .HasColumnName("Old_Price");

                entity.Property(e => e.PoInvoiceNumber)
                    .HasMaxLength(50)
                    .HasColumnName("PO_Invoice_Number");

                entity.Property(e => e.PriceDiff).HasMaxLength(50);

                entity.Property(e => e.PricePercentage).HasMaxLength(50);

                entity.Property(e => e.StockTemNumber)
                    .HasMaxLength(50)
                    .HasColumnName("Stock_tem_Number");
            });

            modelBuilder.Entity<ItemDocument>(entity =>
            {
                entity.HasKey(e => e.DocumentId);

                entity.Property(e => e.DocumentId).HasColumnName("DocumentID");

                entity.Property(e => e.DocumentName).HasMaxLength(50);

                entity.Property(e => e.DocumentType).HasMaxLength(50);

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");
            });

            modelBuilder.Entity<ItemSubCategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.ParentCategoryName).HasMaxLength(50);

                entity.Property(e => e.SubCategory).HasMaxLength(50);
            });

            modelBuilder.Entity<LeaveType>(entity =>
            {
                entity.Property(e => e.LeaveTypeId).HasColumnName("LeaveTypeID");

                entity.Property(e => e.TypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<LedgerCategory>(entity =>
            {
                entity.HasKey(e => e.Ledger);

                entity.ToTable("LedgerCategory");

                entity.Property(e => e.Category).HasMaxLength(50);
            });

            modelBuilder.Entity<MisPick>(entity =>
            {
                entity.ToTable("MisPick");

                entity.Property(e => e.MisPickId).HasColumnName("MisPickID");

                entity.Property(e => e.MisPickName).HasMaxLength(250);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.ToTable("Model");

                entity.Property(e => e.Id).HasColumnName("ID");
            });

            modelBuilder.Entity<OngoingSaleInvoice>(entity =>
            {
                entity.ToTable("OngoingSaleInvoice");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Count).HasMaxLength(250);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DriverId).HasColumnName("DriverID");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.IsPostStatus).HasMaxLength(50);

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.PaidDate).HasColumnType("datetime");

                entity.Property(e => e.Quantity).HasMaxLength(250);

                entity.Property(e => e.SalesManagerId).HasColumnName("SalesManagerID");

                entity.Property(e => e.SalesmanId).HasColumnName("SalesmanID");

                entity.Property(e => e.ShippedName).HasMaxLength(250);

                entity.Property(e => e.ShippedViaId).HasColumnName("ShippedViaID");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");

                entity.Property(e => e.WareHouseId).HasColumnName("WareHouseID");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Cashier).HasMaxLength(50);

                entity.Property(e => e.DeliveredDate).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasMaxLength(50);

                entity.Property(e => e.GrossAmount).HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentType).HasMaxLength(50);

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.ProductVariation).HasMaxLength(50);

                entity.Property(e => e.Quantity).HasMaxLength(50);

                entity.Property(e => e.RetailPrice).HasMaxLength(50);

                entity.Property(e => e.Tax).HasMaxLength(50);

                entity.Property(e => e.TerminalNumber).HasMaxLength(50);

                entity.Property(e => e.TotalUnits).HasMaxLength(50);

                entity.Property(e => e.UnitCharge).HasMaxLength(50);
            });

            modelBuilder.Entity<PayExpense>(entity =>
            {
                entity.ToTable("PayExpense");

                entity.Property(e => e.PayExpenseId).HasColumnName("PayExpense_ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountName).HasMaxLength(50);

                entity.Property(e => e.CashBalance).HasMaxLength(50);

                entity.Property(e => e.CashierUser).HasMaxLength(50);

                entity.Property(e => e.CheckDate).HasColumnType("datetime");

                entity.Property(e => e.CheckNumber).HasMaxLength(50);

                entity.Property(e => e.CheckTitle).HasMaxLength(50);

                entity.Property(e => e.Credit).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Debit).HasMaxLength(50);

                entity.Property(e => e.Discount).HasMaxLength(50);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNumber).HasMaxLength(50);

                entity.Property(e => e.InvoiceTypeId)
                    .HasMaxLength(50)
                    .HasColumnName("InvoiceType_ID");

                entity.Property(e => e.NetAmount).HasMaxLength(50);

                entity.Property(e => e.PayFromAccount).HasMaxLength(50);

                entity.Property(e => e.PayFromAccountId).HasColumnName("PayFromAccountID");

                entity.Property(e => e.PaymentType).HasMaxLength(50);

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.StoreName).HasMaxLength(50);

                entity.Property(e => e.Tax).HasMaxLength(50);

                entity.Property(e => e.TaxAmount).HasMaxLength(50);
            });

            modelBuilder.Entity<Payable>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountName).HasMaxLength(50);
            });

            modelBuilder.Entity<Paying>(entity =>
            {
                entity.ToTable("Paying");

                entity.Property(e => e.PayingId).HasColumnName("Paying_ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountName).HasMaxLength(50);

                entity.Property(e => e.CashBalance).HasMaxLength(50);

                entity.Property(e => e.CashierUser).HasMaxLength(50);

                entity.Property(e => e.CheckDate).HasColumnType("datetime");

                entity.Property(e => e.CheckNumber).HasMaxLength(50);

                entity.Property(e => e.CheckTitle).HasMaxLength(50);

                entity.Property(e => e.Credit).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Debit).HasMaxLength(50);

                entity.Property(e => e.Discount).HasMaxLength(50);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNumber).HasMaxLength(50);

                entity.Property(e => e.InvoiceTypeId)
                    .HasMaxLength(50)
                    .HasColumnName("InvoiceType_ID");

                entity.Property(e => e.NetAmount).HasMaxLength(50);

                entity.Property(e => e.PayFromAccount).HasMaxLength(50);

                entity.Property(e => e.PayFromAccountId).HasColumnName("PayFromAccountID");

                entity.Property(e => e.PaymentType).HasMaxLength(50);

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.StoreName).HasMaxLength(50);

                entity.Property(e => e.Tax).HasMaxLength(50);

                entity.Property(e => e.TaxAmount).HasMaxLength(50);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.ByUser).HasMaxLength(50);

                entity.Property(e => e.CustomerName).HasMaxLength(50);

                entity.Property(e => e.OrderBy).HasMaxLength(50);

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PaymentDetail>(entity =>
            {
                entity.ToTable("PaymentDetail");

                entity.Property(e => e.CkcardNumber).HasColumnName("CKCardNumber");

                entity.Property(e => e.HoldDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PaymentTerm>(entity =>
            {
                entity.ToTable("PaymentTerm");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<PersonPin>(entity =>
            {
                entity.HasKey(e => e.PinId);

                entity.ToTable("PersonPin");

                entity.Property(e => e.PinId).HasColumnName("PinID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.EmployeeNumber).HasMaxLength(250);

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.TerminalId).HasColumnName("TerminalID");

                entity.Property(e => e.TerminalName).HasMaxLength(250);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<PosSale>(entity =>
            {
                entity.ToTable("PosSale");

                entity.Property(e => e.PossaleId).HasColumnName("POSSaleID");

                entity.Property(e => e.Cost).HasMaxLength(50);

                entity.Property(e => e.Count).HasMaxLength(250);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DamageQty).HasMaxLength(50);

                entity.Property(e => e.DriverId).HasColumnName("DriverID");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.IsPostStatus).HasMaxLength(50);

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.PaidDate).HasColumnType("datetime");

                entity.Property(e => e.Quantity).HasMaxLength(250);

                entity.Property(e => e.ReturnQty).HasMaxLength(50);

                entity.Property(e => e.RingerQuantity).HasMaxLength(50);

                entity.Property(e => e.SalesManagerId).HasColumnName("SalesManagerID");

                entity.Property(e => e.SalesmanId).HasColumnName("SalesmanID");

                entity.Property(e => e.ShipmentLimit).HasMaxLength(50);

                entity.Property(e => e.ShippedName).HasMaxLength(250);

                entity.Property(e => e.ShippedViaId).HasColumnName("ShippedViaID");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");

                entity.Property(e => e.WareHouseId).HasColumnName("WareHouseID");
            });

            modelBuilder.Entity<Pricing>(entity =>
            {
                entity.ToTable("Pricing");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddtoCostPercenatge).HasMaxLength(50);

                entity.Property(e => e.Adjustment).HasMaxLength(50);

                entity.Property(e => e.AllowECommerce).HasColumnName("AllowE-Commerce");

                entity.Property(e => e.ArticleId).HasColumnName("ArticleID");

                entity.Property(e => e.BrandId).HasColumnName("BrandID");

                entity.Property(e => e.BrandName).HasMaxLength(50);

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.ColorId).HasColumnName("ColorID");

                entity.Property(e => e.ColorName).HasMaxLength(50);

                entity.Property(e => e.Cost).HasMaxLength(50);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_Date");

                entity.Property(e => e.DiscountPrice).HasMaxLength(50);

                entity.Property(e => e.Family).HasMaxLength(50);

                entity.Property(e => e.FamilyId).HasColumnName("FamilyID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.GroupName).HasMaxLength(50);

                entity.Property(e => e.HighlimitOn).HasMaxLength(50);

                entity.Property(e => e.ItemCategoryId).HasColumnName("ItemCategoryID");

                entity.Property(e => e.ItemImage).HasColumnType("image");

                entity.Property(e => e.ItemNumber).HasMaxLength(50);

                entity.Property(e => e.ItemSubCategoryId).HasColumnName("ItemSubCategoryID");

                entity.Property(e => e.LocationFour).HasMaxLength(250);

                entity.Property(e => e.LocationThree).HasMaxLength(250);

                entity.Property(e => e.LocationTwo).HasMaxLength(250);

                entity.Property(e => e.MaintainStockForDays).HasMaxLength(250);

                entity.Property(e => e.MaxOrderQty).HasMaxLength(50);

                entity.Property(e => e.MfgPromotion).HasMaxLength(50);

                entity.Property(e => e.MinOrderQty).HasMaxLength(50);

                entity.Property(e => e.MisPickId).HasColumnName("MisPickID");

                entity.Property(e => e.ModelId).HasColumnName("ModelID");

                entity.Property(e => e.ModelName).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_Date");

                entity.Property(e => e.MsareportAs)
                    .HasMaxLength(50)
                    .HasColumnName("MSAReportAs");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.OldPrice)
                    .HasMaxLength(50)
                    .HasColumnName("Old_Price");

                entity.Property(e => e.OutofstateCost).HasMaxLength(50);

                entity.Property(e => e.OutofstateRetail).HasMaxLength(50);

                entity.Property(e => e.PriceDiff).HasMaxLength(50);

                entity.Property(e => e.PriceDiffPercentage).HasMaxLength(50);

                entity.Property(e => e.ProductCode).HasMaxLength(50);

                entity.Property(e => e.ProfitPercentage).HasMaxLength(50);

                entity.Property(e => e.QtyUnit).HasMaxLength(50);

                entity.Property(e => e.QtyinStock).HasMaxLength(50);

                entity.Property(e => e.QuantityCase).HasMaxLength(50);

                entity.Property(e => e.QuantityPallet).HasMaxLength(50);

                entity.Property(e => e.ReportingWeight).HasMaxLength(50);

                entity.Property(e => e.Retail).HasMaxLength(50);

                entity.Property(e => e.RetailPackPrice).HasMaxLength(50);

                entity.Property(e => e.SaleRetail).HasMaxLength(50);

                entity.Property(e => e.SalesLimit).HasMaxLength(50);

                entity.Property(e => e.ShipmentLimit).HasMaxLength(50);

                entity.Property(e => e.SingleUnitMsa).HasColumnName("SingleUnitMSA");

                entity.Property(e => e.Size).HasMaxLength(50);

                entity.Property(e => e.Sku)
                    .HasMaxLength(50)
                    .HasColumnName("SKU");

                entity.Property(e => e.StateReportAs).HasMaxLength(50);

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.SttaxOnCost).HasColumnName("STTaxOnCost");

                entity.Property(e => e.SttaxOnSale).HasColumnName("STTaxOnSale");

                entity.Property(e => e.SttaxPerQty)
                    .HasMaxLength(50)
                    .HasColumnName("STTaxPerQty");

                entity.Property(e => e.SttaxRate)
                    .HasMaxLength(50)
                    .HasColumnName("STTaxRate");

                entity.Property(e => e.SubCatName).HasMaxLength(50);

                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.Property(e => e.UnitCharge).HasMaxLength(50);

                entity.Property(e => e.UnitRetail).HasMaxLength(50);

                entity.Property(e => e.Units).HasMaxLength(250);

                entity.Property(e => e.UnitsInPack).HasMaxLength(50);

                entity.Property(e => e.WeightLb)
                    .HasMaxLength(250)
                    .HasColumnName("WeightLB");

                entity.Property(e => e.WeightOz)
                    .HasMaxLength(250)
                    .HasColumnName("WeightOZ");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.ToTable("Provider");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("Purchase");

                entity.Property(e => e.PurchaseDate).HasColumnType("datetime");

                entity.Property(e => e.Quantity).HasMaxLength(500);

                entity.Property(e => e.QuantityPack).HasMaxLength(500);

                entity.Property(e => e.QuantityUnit).HasMaxLength(100);

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.StoreName).HasMaxLength(250);

                entity.Property(e => e.WareHouseId).HasColumnName("WareHouseID");

                entity.Property(e => e.WareHouseName).HasMaxLength(250);
            });

            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.ToTable("PurchaseOrder");

                entity.Property(e => e.CigaretteStick).HasMaxLength(50);

                entity.Property(e => e.Currrentuser).HasMaxLength(50);

                entity.Property(e => e.DateReceived).HasColumnType("datetime");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");

                entity.Property(e => e.IsPostStatus).HasMaxLength(50);

                entity.Property(e => e.PaidDate).HasColumnType("datetime");

                entity.Property(e => e.Podate)
                    .HasColumnType("datetime")
                    .HasColumnName("PODate");

                entity.Property(e => e.ReceivedDate).HasColumnType("datetime");

                entity.Property(e => e.SupplierName).HasMaxLength(50);

                entity.Property(e => e.SupplierNumber).HasMaxLength(50);

                entity.Property(e => e.TotalCigar).HasMaxLength(50);

                entity.Property(e => e.TotalCigarette).HasMaxLength(50);

                entity.Property(e => e.TotalItems).HasMaxLength(50);

                entity.Property(e => e.TotalTobacco).HasMaxLength(50);

                entity.Property(e => e.UpdateOscost).HasColumnName("UpdateOSCost");
            });

            modelBuilder.Entity<Receivable>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountName).HasMaxLength(50);
            });

            modelBuilder.Entity<Receiving>(entity =>
            {
                entity.ToTable("Receiving");

                entity.Property(e => e.ReceivingId).HasColumnName("Receiving_ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountName).HasMaxLength(50);

                entity.Property(e => e.CashBalance).HasMaxLength(50);

                entity.Property(e => e.CashierUser).HasMaxLength(50);

                entity.Property(e => e.CheckDate).HasColumnType("datetime");

                entity.Property(e => e.CheckNumber).HasMaxLength(50);

                entity.Property(e => e.CheckTitle).HasMaxLength(50);

                entity.Property(e => e.Credit).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Debit).HasMaxLength(50);

                entity.Property(e => e.Discount).HasMaxLength(50);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNumber).HasMaxLength(50);

                entity.Property(e => e.InvoiceTypeId)
                    .HasMaxLength(50)
                    .HasColumnName("InvoiceType_ID");

                entity.Property(e => e.NetAmount).HasMaxLength(50);

                entity.Property(e => e.PayFromAccount).HasMaxLength(50);

                entity.Property(e => e.PayFromAccountId).HasColumnName("PayFromAccountID");

                entity.Property(e => e.PaymentType).HasMaxLength(50);

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.StoreName).HasMaxLength(50);

                entity.Property(e => e.Tax).HasMaxLength(50);

                entity.Property(e => e.TaxAmount).HasMaxLength(50);
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.ToTable("Route");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.RouteName).HasMaxLength(250);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.SaleId).HasColumnName("SaleID");

                entity.Property(e => e.BarCode).HasMaxLength(250);

                entity.Property(e => e.Cost).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasMaxLength(250);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Damage).HasMaxLength(50);

                entity.Property(e => e.DiscountCash).HasMaxLength(50);

                entity.Property(e => e.DiscountPercentage).HasMaxLength(50);

                entity.Property(e => e.InvoiceNumber).HasMaxLength(250);

                entity.Property(e => e.ItemCode).HasMaxLength(250);

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.ItemName).HasMaxLength(250);

                entity.Property(e => e.Quantity).HasMaxLength(250);

                entity.Property(e => e.QuantityId).HasColumnName("QuantityID");

                entity.Property(e => e.QuantityUnit).HasMaxLength(50);

                entity.Property(e => e.SaleDate).HasColumnType("datetime");

                entity.Property(e => e.SalesManagerCreditAllow).HasMaxLength(50);

                entity.Property(e => e.SalesManagerId).HasColumnName("SalesManagerID");

                entity.Property(e => e.Sku)
                    .HasMaxLength(250)
                    .HasColumnName("SKU");

                entity.Property(e => e.StoreId).HasColumnName("StoreID");

                entity.Property(e => e.StoreName).HasMaxLength(250);

                entity.Property(e => e.SupervisorCreditAllow).HasMaxLength(50);

                entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");

                entity.Property(e => e.TaxCash).HasMaxLength(50);

                entity.Property(e => e.TaxPerchange).HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasMaxLength(250);

                entity.Property(e => e.WareHouseId).HasColumnName("WareHouseID");

                entity.Property(e => e.WareHouseName).HasMaxLength(250);
            });

            modelBuilder.Entity<SaleHistory>(entity =>
            {
                entity.ToTable("SaleHistory");

                entity.Property(e => e.SaleHistoryId).HasColumnName("SaleHistoryID");

                entity.Property(e => e.CartDetailId)
                    .HasMaxLength(50)
                    .HasColumnName("CartDetailID");

                entity.Property(e => e.ItemImage).HasColumnType("image");

                entity.Property(e => e.ItemNumber).HasMaxLength(50);

                entity.Property(e => e.PosSaleId).HasColumnName("PosSaleID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.TicketId).HasMaxLength(50);

                entity.Property(e => e.TransacetionId).HasColumnName("TransacetionID");
            });

            modelBuilder.Entity<SalesInvTransaction>(entity =>
            {
                entity.Property(e => e.HoldDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SalesInvoice>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<SalesManager>(entity =>
            {
                entity.HasKey(e => e.SaleManagerId);

                entity.ToTable("SalesManager");

                entity.Property(e => e.AccessPin).HasMaxLength(50);

                entity.Property(e => e.CreditLimit).HasMaxLength(50);

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Salesman>(entity =>
            {
                entity.ToTable("Salesman");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.City).HasMaxLength(250);

                entity.Property(e => e.Country).HasMaxLength(250);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Mobile).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(250);

                entity.Property(e => e.State).HasMaxLength(250);
            });

            modelBuilder.Entity<SecurityKey>(entity =>
            {
                entity.Property(e => e.SecurityKeyId).HasColumnName("SecurityKeyID");

                entity.Property(e => e.CreadtedDate).HasColumnType("datetime");

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.UserEmail).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<ShipmentPurchase>(entity =>
            {
                entity.ToTable("ShipmentPurchase");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Reference).HasMaxLength(250);

                entity.Property(e => e.ShipNumber).HasMaxLength(250);

                entity.Property(e => e.Type).HasMaxLength(250);
            });

            modelBuilder.Entity<ShiptoReference>(entity =>
            {
                entity.ToTable("ShiptoReference");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.StateName).HasMaxLength(50);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.StoreName).HasMaxLength(50);
            });

            modelBuilder.Entity<Sttax>(entity =>
            {
                entity.HasKey(e => e.TaxId);

                entity.ToTable("STTax");

                entity.Property(e => e.TaxId).HasColumnName("TaxID");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasMaxLength(50);

                entity.Property(e => e.PerOz)
                    .HasMaxLength(50)
                    .HasColumnName("PerOZ");

                entity.Property(e => e.PerQty).HasMaxLength(50);

                entity.Property(e => e.PerUcount)
                    .HasMaxLength(50)
                    .HasColumnName("PerUCount");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductName).HasMaxLength(50);

                entity.Property(e => e.SttaxOnCost).HasColumnName("STTaxOnCost");

                entity.Property(e => e.SttaxOnSale).HasColumnName("STTaxOnSale");

                entity.Property(e => e.Tax).HasMaxLength(50);
            });

            modelBuilder.Entity<SubGroup>(entity =>
            {
                entity.ToTable("SubGroup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.ParentGroupName).HasMaxLength(250);

                entity.Property(e => e.SubGroupName).HasMaxLength(250);
            });

            modelBuilder.Entity<Supervisor>(entity =>
            {
                entity.ToTable("Supervisor");

                entity.Property(e => e.AccessPin).HasMaxLength(50);

                entity.Property(e => e.CreditLimit).HasMaxLength(50);

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<SupervisorCredit>(entity =>
            {
                entity.HasKey(e => e.CreditId);

                entity.ToTable("SupervisorCredit");

                entity.Property(e => e.CreditAmount).HasMaxLength(50);

                entity.Property(e => e.CreditDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentStatus).HasDefaultValueSql("((0))");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<SupplierCreditPayment>(entity =>
            {
                entity.HasKey(e => e.CreditPaymentId);

                entity.ToTable("SupplierCreditPayment");

                entity.Property(e => e.CreditPaymentId).HasColumnName("CreditPaymentID");

                entity.Property(e => e.CreditDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.Pobalance).HasColumnName("POBalance");

                entity.Property(e => e.Ponumber).HasColumnName("PONumber");
            });

            modelBuilder.Entity<SupplierDocument>(entity =>
            {
                entity.HasKey(e => e.DocumentId);

                entity.Property(e => e.DocumentId).HasColumnName("DocumentID");

                entity.Property(e => e.DocumentName).HasMaxLength(50);

                entity.Property(e => e.DocumentType).HasMaxLength(50);

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            });

            modelBuilder.Entity<SupplierDocumentType>(entity =>
            {
                entity.HasKey(e => e.DocumentTypeId);

                entity.ToTable("SupplierDocumentType");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.DocumentType).HasMaxLength(50);
            });

            modelBuilder.Entity<SupplierItemNumber>(entity =>
            {
                entity.ToTable("SupplierItemNumber");

                entity.Property(e => e.SupplierItemNumberId).HasColumnName("SupplierItemNumberID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.VendorId).HasColumnName("VendorID");
            });

            modelBuilder.Entity<SupplierOtherPayment>(entity =>
            {
                entity.HasKey(e => e.OtherPaymentId);

                entity.ToTable("SupplierOtherPayment");

                entity.Property(e => e.OtherPaymentId).HasColumnName("OtherPaymentID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Pobalance).HasColumnName("POBalance");

                entity.Property(e => e.Ponumber).HasColumnName("PONumber");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            });

            modelBuilder.Entity<SupplierType>(entity =>
            {
                entity.HasKey(e => e.VendorId);

                entity.ToTable("SupplierType");

                entity.Property(e => e.VendorId).HasColumnName("VendorID");

                entity.Property(e => e.VendorType).HasMaxLength(50);
            });

            modelBuilder.Entity<SuppliersPayment>(entity =>
            {
                entity.HasKey(e => e.SupplierPaymentId);

                entity.Property(e => e.SupplierPaymentId).HasColumnName("SupplierPaymentID");

                entity.Property(e => e.AddedBy).HasMaxLength(50);

                entity.Property(e => e.PaidDate).HasColumnType("datetime");

                entity.Property(e => e.Pobalance).HasColumnName("POBalance");

                entity.Property(e => e.Ponumber).HasColumnName("PONumber");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            });

            modelBuilder.Entity<Terminal>(entity =>
            {
                entity.ToTable("Terminal");

                entity.Property(e => e.TerminalId).HasColumnName("TerminalID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.SecurityPin).HasMaxLength(50);

                entity.Property(e => e.TerminalName).HasMaxLength(250);

                entity.Property(e => e.TerminalNumber).HasMaxLength(250);
            });

            modelBuilder.Entity<TerminalAccess>(entity =>
            {
                entity.ToTable("TerminalAccess");

                entity.Property(e => e.TerminalAccessId).HasColumnName("TerminalAccessID");

                entity.Property(e => e.AccessByUser).HasMaxLength(50);

                entity.Property(e => e.AccessDate).HasColumnType("datetime");

                entity.Property(e => e.AccessUserId).HasColumnName("AccessUserID");

                entity.Property(e => e.CloseTime).HasMaxLength(50);

                entity.Property(e => e.StartTime).HasMaxLength(50);

                entity.Property(e => e.TerminalId).HasColumnName("TerminalID");

                entity.Property(e => e.TerminalNumber).HasMaxLength(250);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.AccountName).HasMaxLength(50);

                entity.Property(e => e.Amount).HasMaxLength(50);

                entity.Property(e => e.ClosingBalance).HasMaxLength(50);

                entity.Property(e => e.Credit).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Debit).HasMaxLength(50);

                entity.Property(e => e.DetailAccountId).HasColumnName("DetailAccountID");

                entity.Property(e => e.InvoiceNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.Property(e => e.VendorId).HasColumnName("VendorID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Irs).HasColumnName("IRS");

                entity.Property(e => e.ProfileImage).HasColumnType("image");

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.TaxId).HasColumnName("Tax_ID");
            });

            modelBuilder.Entity<WareHouse>(entity =>
            {
                entity.ToTable("WareHouse");

                entity.Property(e => e.WareHouseId).HasColumnName("WareHouseID");

                entity.Property(e => e.ConcernedPersonName).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.WareHouseName).HasMaxLength(50);
            });

            modelBuilder.Entity<ZipCode>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City).HasMaxLength(250);

                entity.Property(e => e.Code).HasMaxLength(250);

                entity.Property(e => e.State).HasMaxLength(250);

                entity.Property(e => e.StateShortcut).HasMaxLength(250);
            });

            modelBuilder.Entity<Zone>(entity =>
            {
                entity.ToTable("Zone");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
