using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;


namespace MineBankSystemPart2
{
    internal class Program
    {

        // Account Request Queue
        // Stores pending account creation requests in FIFO order 
        public static Queue<string> requestCreateAccountsInfo = new Queue<string>();

        // Customer Reviews Stack
        // Stores submitted reviews in LIFO order (most recent first)
        public static Stack<string> submittedReviews = new Stack<string>();

        public static Queue<int> adm = new Queue<int>();


        // Unique account identifiers
        public static List<int> accountNumbers = new List<int>();
        // Customer full names
        public static List<string> accountNames = new List<string>();
        // Current account balances
        public static List<double> balances = new List<double>();

        public static Queue<string> pendingPasswords = new Queue<string>();
        public static string pendingPasswordsPath = "pending_passwords.txt";
        public static void SavePendingPasswords()
        {
            try
            {
                File.WriteAllLines(pendingPasswordsPath, pendingPasswords);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving pending passwords: {ex.Message}");
            }
        }
        public static void LoadPendingPasswords()
        {
            try
            {
                if (File.Exists(pendingPasswordsPath))
                {
                    pendingPasswords = new Queue<string>(File.ReadAllLines(pendingPasswordsPath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading pending passwords: {ex.Message}");
            }
        }

        // Links to accountNumbers

        public static List<int> transactionAccountNumbers = new List<int>();
        // Format: yyyy-MM-dd HH:mm:ss
        public static List<string> transactionTimestamps = new List<string>();
        // "DEPOSIT"/"WITHDRAWAL"
        public static List<string> transactionTypes = new List<string>();
        // Transaction value
        public static List<double> transactionAmounts = new List<double>();
        // Balance after transaction
        public static List<double> transactionBalances = new List<double>();

        // Add to your existing field declarations
        public static List<string> accountPasswords = new List<string>(); // Store hashed passwords
        public static string passwordPath = "passwords.txt"; // File to store hashed passwords

        // Add these with your other static field declarations
        public static List<string> phoneNumbers = new List<string>();
        public static List<string> addresses = new List<string>();

        // Account security fields
        public static List<int> failedLoginAttempts = new List<int>();
        public static List<bool> accountLocked = new List<bool>();
        public static List<bool> accountLockedStatus = new List<bool>();
        public const int MAX_LOGIN_ATTEMPTS = 3;

        // Loan related fields
        public static Queue<string> loanRequests = new Queue<string>();
        public static List<int> loanAccountNumbers = new List<int>();
        public static List<double> loanAmounts = new List<double>();
        public static List<double> interestRates = new List<double>();
        public static List<bool> loanApprovedStatus = new List<bool>();
        public static string loansFilePath = "loans.txt";
        public const double MIN_BALANCE_FOR_LOAN = 5000.00;
        public const double DEFAULT_INTEREST_RATE = 5.0; // 5% interest

        // Password requirements
        public const int MIN_PASSWORD_LENGTH = 8;
        public const int MAX_PASSWORD_LENGTH = 20;

        // Change these declarations:
        public static string userDataPath = "UserAccounts.txt";
        public static string accountRequestsPath = "AccountRequests.txt";  // New separate file

        // Feedback system fields
        public static List<int> feedbackScores = new List<int>();
        public static List<int> feedbackAccountNumbers = new List<int>();
        public static List<string> feedbackComments = new List<string>();
        public static string feedbackFilePath = "feedback.txt";

        // Customer reviews storage
        public static string pathFile = "reviews.txt";
        // Transaction history storage
        public static string transactionsFile = "transactions.txt";
        // store admin accounts
        public static string pathAdmin = "adm.txt";
        // store national ID path
        public static string nationalIDPath = "nationalIDs.txt";

        // Add with your other static fields
        public const string ADMIN_ID = "1234";
        public const string ADMIN_PASSWORD = "12345"; // Change to a strong password
        public static bool isAdminAuthenticated = false;

        // Base account number (increments for new accounts)
        public static int userCordNumber = 12062000;
        // Starting balance for new accounts
        public static int defaultBalances = 5;

        // Appointment system fields
        public static Queue<string> appointmentQueue = new Queue<string>();
        public static List<int> appointmentAccountNumbers = new List<int>();
        public static string appointmentsFilePath = "appointments.txt";
        public const int MAX_DAYS_IN_ADVANCE = 30;

        // Currency conversion fields
        public static Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>()
{
    {"USD", 0.38m},    // 1 USD = 0.38 OMR
    {"EUR", 0.42m},    // 1 EUR = 0.42 OMR
    {"GBP", 0.49m}     // 1 GBP = 0.49 OMR
};
        // Should already be in your field declarations
        public static List<string> transactionTimestamps1 = new List<string>();
        static void Main(string[] args)
        {
            // display home page in main funaction
            // load list of reviews
            LoadReviews();
            //load user data
            LoadUserData();
            // load transactions 
            LoadTransactions();
            // load account requests
            LoadAccountRequest();
            //load admin
            LoadAdmi();

            LoadAppointments();
            LoadPasswords();
            LoadPendingPasswords();
            LoadLoans();

            // display home page in main funaction
            HomePage();

            // Prompt for backup before exiting
            Console.Clear();
            Console.WriteLine("Would you like to create a backup before exiting? (Y/N)");
            string response = Console.ReadLine()?.Trim().ToUpper();

            if (response == "Y")
            {
                CreateBackup();
            }

            Console.WriteLine("Thank you for using One Piece Bank System. Goodbye!");



        }



        public static void HomePage()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("         Welcome To One Piece Bank System          \n ");
            Console.WriteLine("**************************************************************\n");

            bool Flag = true;
            while (Flag)
            {
                Console.WriteLine("Select One Option");
                Console.WriteLine("1- User Login");
                Console.WriteLine("2- Admin Login");
                Console.WriteLine("3- Exit");

                Console.Write("Enter your choice: ");

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number (1-3).");
                    Console.Write("Enter your choice: ");
                }

                switch (choice)
                {
                    case 1: // User Login
                        UserLogin();
                        break;
                    case 2: // Admin Login
                        AdminLogin();
                        break;
                    case 3: // Exit
                        Console.Clear();
                        Console.WriteLine("Thank you for using One Piece Bank System. Goodbye!");
                        Flag = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid option. Please try again.\n");
                        break;
                }
            }
        }

        public static void UserLogin()
        {
            try
            {
                Console.Clear();
                DisplayLoginHeader();

                // Get and validate account number
                int? accountNumber = GetAccountNumber();
                if (!accountNumber.HasValue) return;

                // Check account existence and handle new account creation
                int? accountIndex = VerifyAccountExistence(accountNumber.Value);
                if (!accountIndex.HasValue) return;

                // Check if account is locked
                if (IsAccountLocked(accountIndex.Value))
                {
                    DisplayLockedAccountMessage();
                    return;
                }

                // Handle authentication
                AuthenticateUser(accountIndex.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}");
                PauseAndContinue();
            }
        }

        private static void DisplayLoginHeader()
        {
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                      USER LOGIN PAGE                         ");
            Console.WriteLine("**************************************************************\n");
        }

        private static int? GetAccountNumber()
        {
            Console.Write("Enter Account Number: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int accountNumber))
            {
                Console.WriteLine("\nInvalid account number format. Only numbers are allowed.");
                PauseAndContinue();
                return null;
            }

            return accountNumber;
        }

        private static int? VerifyAccountExistence(int accountNumber)
        {
            if (accountNumbers.Contains(accountNumber))
            {
                return accountNumbers.IndexOf(accountNumber);
            }

            Console.WriteLine("\nAccount not found.");
            Console.Write("Would you like to create a new account? (Y/N): ");
            string response = Console.ReadLine()?.Trim().ToUpper();

            if (response == "Y")
            {
                requestCreateAccounts();
            }
            else
            {
                Console.WriteLine("\nReturning to main menu...");
                PauseAndContinue();
            }

            return null;
        }

        private static bool IsAccountLocked(int accountIndex)
        {
            return accountIndex < accountLocked.Count && accountLocked[accountIndex];
        }

        private static void DisplayLockedAccountMessage()
        {
            Console.WriteLine("\nThis account is locked due to multiple failed login attempts.");
            Console.WriteLine("Please contact admin to unlock your account.");
            PauseAndContinue();
        }

        private static void AuthenticateUser(int accountIndex)
        {
            for (int attempt = 1; attempt <= 3; attempt++)
            {
                Console.Write("Enter Password: ");
                string password = ReadMaskedPassword();
                string hashedPassword = HashPassword(password);

                if (IsPasswordCorrect(accountIndex, hashedPassword))
                {
                    HandleSuccessfulLogin(accountIndex);
                    return;
                }

                HandleFailedAttempt(accountIndex, attempt);
            }

            Console.WriteLine("\nPress any key to return to main menu...");
            Console.ReadKey();
        }

        private static bool IsPasswordCorrect(int accountIndex, string hashedPassword)
        {
            return accountIndex < accountPasswords.Count &&
                   accountPasswords[accountIndex] == hashedPassword;
        }

        private static void HandleSuccessfulLogin(int accountIndex)
        {
            Console.WriteLine("\nLogin successful!");

            // Reset failed attempts
            if (accountIndex < failedLoginAttempts.Count)
            {
                failedLoginAttempts[accountIndex] = 0;
            }

            PauseAndContinue();
            UserIU();
        }

        private static void HandleFailedAttempt(int accountIndex, int attempt)
        {
            Console.WriteLine($"\nInvalid password. Attempt {attempt}/3");

            // Update failed attempts counter
            if (accountIndex < failedLoginAttempts.Count)
            {
                failedLoginAttempts[accountIndex]++;
            }

            // Lock account if max attempts reached
            if (attempt >= 3 && accountIndex < accountLocked.Count)
            {
                accountLocked[accountIndex] = true;
                Console.WriteLine("\nAccount locked due to 3 failed attempts.");
                Console.WriteLine("Please contact admin to unlock.");
            }
        }

        private static void PauseAndContinue()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }


        public static void AdminLogin()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                      ADMIN AUTHENTICATION                    ");
            Console.WriteLine("**************************************************************");

            int attempts = 0;
            const int maxAttempts = 3;

            while (attempts < maxAttempts)
            {
                Console.Write("\nEnter Admin ID: ");
                string adminId = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = ReadMaskedPassword(); // Using existing password masking

                if (adminId == ADMIN_ID && password == ADMIN_PASSWORD)
                {
                    isAdminAuthenticated = true;
                    Console.WriteLine("\nAuthentication successful!");
                    Console.WriteLine("\nPress any key to continue to admin menu...");
                    Console.ReadKey();
                    AdminIU();
                    return;
                }
                else
                {
                    attempts++;
                    int remainingAttempts = maxAttempts - attempts;
                    Console.WriteLine($"\nInvalid credentials. {remainingAttempts} attempts remaining.");
                }
            }
              //fuk you 
            Console.WriteLine("\nMaximum attempts reached. Returning to main menu...");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }


        //++++++++++++++++++++++++++++++++ Login +++++++++++++++++++++++++++++++++++++++++








        //++++++++++++++++++++++++++++++++ User Interface +++++++++++++++++++++++++++++++++++++++++
        public static void UserIU()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                      CUSTOMER DASHBOARD                       \n");
            Console.WriteLine("**************************************************************\n");

            bool Flag = true;
            while (Flag)
            {
                Console.WriteLine("\nSelect an Option:");
                Console.WriteLine(" 1. Account Management");
                Console.WriteLine(" 2. Transactions");
                Console.WriteLine(" 3. Financial Services");
                Console.WriteLine(" 4. Book Appointment");
                Console.WriteLine(" 5. Service Feedback");
                Console.WriteLine(" 0. Logout");

                Console.Write("\nEnter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int mainChoice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                switch (mainChoice)
                {
                    case 1: // Account Management
                        Console.Clear();
                        Console.WriteLine("ACCOUNT MANAGEMENT\n");
                        Console.WriteLine("1. Create New Account");
                        Console.WriteLine("2. View Account Balance");
                        Console.WriteLine("3. Update Account Information");
                        Console.WriteLine("0. Back");
                        Console.Write("\nSelect option: ");

                        int accountChoice = int.Parse(Console.ReadLine());
                        switch (accountChoice)
                        {
                            case 1: requestCreateAccounts(); break;
                            case 2: viewBalances(); break;
                            case 3: UpdateAccountInfo(); break;
                            case 0: break;
                            default: Console.WriteLine("Invalid option"); break;
                        }
                        break;

                    case 2: // Transactions
                        Console.Clear();
                        Console.WriteLine("TRANSACTIONS\n");
                        Console.WriteLine("1. Deposit Money");
                        Console.WriteLine("2. Withdraw Money");
                        Console.WriteLine("3. View Full Transaction History");
                        Console.WriteLine("4. View Recent Transactions");
                        Console.WriteLine("5. Generate Monthly Statement");
                        Console.WriteLine("0. Back");
                        Console.Write("\nSelect option: ");

                        int transactionChoice = int.Parse(Console.ReadLine());
                        switch (transactionChoice)
                        {
                            case 1: depostitMoney(); break;
                            case 2: withdrawMoney(); break;
                            case 3: viewTransactionHistory(); break;
                            case 4: ViewFilteredTransactions(); break;
                            case 5: GenerateMonthlyStatement(); break;
                            case 0: break;
                            default: Console.WriteLine("Invalid option"); break;
                        }
                        break;

                    case 3: // Financial Services
                        Console.Clear();
                        Console.WriteLine("FINANCIAL SERVICES\n");
                        Console.WriteLine("1. Request Loan");
                        Console.WriteLine("2. View Active Loans");
                        Console.WriteLine("0. Back");
                        Console.Write("\nSelect option: ");

                        int serviceChoice = int.Parse(Console.ReadLine());
                        switch (serviceChoice)
                        {
                            case 1: RequestLoan(); break;
                            case 2: ViewUserLoans(); break;
                            case 0: break;
                            default: Console.WriteLine("Invalid option"); break;
                        }
                        break;

                    case 4: // Book Appointment
                        BookAppointment();
                        break;

                    case 5: // Service Feedback
                        Console.Clear();
                        Console.WriteLine("SERVICE FEEDBACK\n");
                        Console.WriteLine("1. Submit Review");
                        Console.WriteLine("2. Rate Recent Transaction");
                        Console.WriteLine("0. Back");
                        Console.Write("\nSelect option: ");

                        int feedbackChoice = int.Parse(Console.ReadLine());
                        switch (feedbackChoice)
                        {
                            case 1: submitReview(); break;
                            case 2: CollectFeedback(GetCurrentAccountNumber()); break;
                            case 0: break;
                            default: Console.WriteLine("Invalid option"); break;
                        }
                        break;

                    case 0: // Logout
                        Console.WriteLine("\nLogging out...");
                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Flag = false;
                        break;

                    default:
                        Console.WriteLine("\nInvalid option. Please try again.");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;
                }

                Console.Clear();
                if (Flag) // Only show header if staying in user menu
                {
                    Console.WriteLine("**************************************************************\n");
                    Console.WriteLine("                      CUSTOMER DASHBOARD                       \n");
                    Console.WriteLine("**************************************************************\n");
                }
            }
        }

        // Helper method to get current user's account number
        public static int GetCurrentAccountNumber()
        {
            // Implementation depends on how you track logged-in users
            // This is just a placeholder - adapt to your actual authentication system
            Console.Write("Enter your account number: ");
            return int.Parse(Console.ReadLine());
        }

        // Method to view user's active loans
        public static void ViewUserLoans()
        {
            Console.Write("Enter your account number: ");
            int accountNumber = int.Parse(Console.ReadLine());

            Console.Clear();
            Console.WriteLine("YOUR ACTIVE LOANS\n");
            Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15}", "Loan ID", "Amount", "Interest", "Status");
            Console.WriteLine(new string('-', 60));

            bool hasLoans = false;
            for (int i = 0; i < loanAccountNumbers.Count; i++)
            {
                if (loanAccountNumbers[i] == accountNumber && loanApprovedStatus[i])
                {
                    Console.WriteLine("{0,-15} {1,-15:C2} {2,-15}% {3,-15}",
                        i + 1, loanAmounts[i], interestRates[i], "Active");
                    hasLoans = true;
                }
            }

            if (!hasLoans)
            {
                Console.WriteLine("No active loans found.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // 1 ########################## Request For Create Account ################################### 

        public static void requestCreateAccounts()
        {
            try
            {
                Console.Clear();
                DisplayAccountCreationHeader();

                // Collect user information with validation
                string name = GetValidName();
                if (string.IsNullOrEmpty(name)) return;

                string nationalID = GetValidNationalID();
                if (string.IsNullOrEmpty(nationalID)) return;

                string password = GetValidPassword();
                if (string.IsNullOrEmpty(password)) return;

             

                string phone = GetPhoneNumber();
                string address = GetAddress();

                // Create and store account request
                CreateAccountRequest(name, nationalID, phone, address, password);

                Console.WriteLine("\nYour account request has been submitted for admin approval.");
                PauseAndContinue();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn error occurred during account creation: {ex.Message}");
                PauseAndContinue();
            }
        }

        private static void DisplayAccountCreationHeader()
        {
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                  REQUEST CREATE ACCOUNT PAGE                 ");
            Console.WriteLine("**************************************************************\n");
        }

        private static string GetValidName()
        {
            while (true)
            {
                Console.Write("Enter Name: ");
                string name = Console.ReadLine()?.Trim();

                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }

                Console.WriteLine("Name cannot be empty. Please try again.");
            }
        }

        private static string GetValidNationalID()
        {
            while (true)
            {
                Console.Write("Enter National ID: ");
                string nationalID = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(nationalID))
                {
                    Console.WriteLine("National ID cannot be empty. Please try again.");
                    continue;
                }

                if (IsDuplicateNationalID(nationalID))
                {
                    Console.WriteLine("This National ID is already registered. Please use a different one.");
                    continue;
                }

                return nationalID;
            }
        }

        private static string GetValidPassword()
        {
            while (true)
            {
                Console.Write("Enter password (8-20 chars, at least 1 letter and 1 number): ");
                string password = ReadMaskedPassword();

                if (ValidatePassword(password))
                {
                    return password;
                }

                Console.WriteLine("\nInvalid password. Must contain:");
                Console.WriteLine("- 8-20 characters");
                Console.WriteLine("- At least 1 letter");
                Console.WriteLine("- At least 1 number");
            }
        }

   

        private static string GetPhoneNumber()
        {
            Console.Write("Enter phone number: ");
            return Console.ReadLine()?.Trim();
        }

        private static string GetAddress()
        {
            Console.Write("Enter address: ");
            return Console.ReadLine()?.Trim();
        }

        private static void CreateAccountRequest(string name, string nationalID, string phone, string address, string password)
        {
            // Store account information
            string accountRequest = $"{name}|{nationalID}|{phone}|{address}";
            requestCreateAccountsInfo.Enqueue(accountRequest);

            // Store hashed password
            string hashedPassword = HashPassword(password);
            pendingPasswords.Enqueue(hashedPassword);

            // Save the national ID to prevent duplicates
            SaveNationalID(nationalID);

            // Initialize security settings
            failedLoginAttempts.Add(0);
            accountLocked.Add(false);

            // Persist data
            SaveAccountRequest();
            SavePendingPasswords();
            SavePasswords();
        }

     



        // Check for duplicate national IDs
        public static bool IsDuplicateNationalID(string nationalID)
        {
            if (!File.Exists(nationalIDPath)) return false;

            string[] existingIDs = File.ReadAllLines(nationalIDPath);
            return existingIDs.Contains(nationalID);
        }

        // Save a new national ID
        public static void SaveNationalID(string nationalID)
        {
            try
            {
                File.AppendAllText(nationalIDPath, nationalID + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving national ID: {ex.Message}");
            }
        }

        // Load all national IDs (for verification)
        public static List<string> LoadNationalIDs()
        {
            List<string> ids = new List<string>();
            try
            {
                if (File.Exists(nationalIDPath))
                {
                    ids = File.ReadAllLines(nationalIDPath).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading national IDs: {ex.Message}");
            }
            return ids;
        }





        //  Prevent Duplicate Account Requests
        public static bool IsDuplicateRequest(string name, int idNumber)
        {
            foreach (var request in requestCreateAccountsInfo)
            {
                string[] parts = request.Split('|');
                if (parts.Length >= 2 && parts[0] == name && int.Parse(parts[1]) == idNumber)
                {
                    return true;
                }
            }
            return false;
        }



        //############################################## Deposit Money #####################################################
        public static void depostitMoney()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                          DEPOSIT MONEY                        ");
            Console.WriteLine("**************************************************************");

            // Get account number
            Console.Write("\nEnter Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Check if account exists
            if (!accountNumbers.Contains(accountNumber))
            {
                Console.WriteLine("Account not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Currency selection
            Console.WriteLine("\nSelect Currency:");
            Console.WriteLine("1. OMR (Local Currency)");
            Console.WriteLine("2. USD");
            Console.WriteLine("3. EUR");
            Console.WriteLine("4. GBP");
            Console.Write("Enter choice (1-4): ");

            string currencyCode = "OMR";
            decimal exchangeRate = 1.0m;

            if (int.TryParse(Console.ReadLine(), out int currencyChoice))
            {
                switch (currencyChoice)
                {
                    case 1:
                        currencyCode = "OMR";
                        break;
                    case 2:
                        currencyCode = "USD";
                        exchangeRate = exchangeRates["USD"];
                        break;
                    case 3:
                        currencyCode = "EUR";
                        exchangeRate = exchangeRates["EUR"];
                        break;
                    case 4:
                        currencyCode = "GBP";
                        exchangeRate = exchangeRates["GBP"];
                        break;
                    default:
                        Console.WriteLine("Invalid choice, using OMR by default.");
                        break;
                }
            }

            // Get amount to deposit
            Console.Write($"\nEnter Amount to Deposit ({currencyCode}): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal foreignAmount) || foreignAmount <= 0)
            {
                Console.WriteLine("Invalid amount!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Convert to local currency
            decimal localAmount = currencyCode == "OMR" ? foreignAmount : foreignAmount * exchangeRate;
            int accountIndex = accountNumbers.IndexOf(accountNumber);

            // Display conversion details
            Console.WriteLine("\nConversion Details:");
            Console.WriteLine($"{foreignAmount:N2} {currencyCode} = {localAmount:N2} OMR");
            Console.WriteLine($"Exchange Rate: 1 {currencyCode} = {exchangeRate:N4} OMR");

            // Confirm deposit
            Console.Write("\nConfirm deposit? (Y/N): ");
            if (Console.ReadLine().ToUpper() != "Y")
            {
                Console.WriteLine("Deposit cancelled.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Process deposit
            balances[accountIndex] += (double)localAmount;

            // Record transaction with currency information
            string transactionType = $"DEPOSIT_{currencyCode}";
            AddTransactionWithCurrency(accountNumber, transactionType, (double)foreignAmount,
                                     (double)localAmount, balances[accountIndex], currencyCode);

            Console.WriteLine($"\nSuccessfully deposited {localAmount:N2} OMR " +
                             $"(from {foreignAmount:N2} {currencyCode})");
            Console.WriteLine($"New balance: {balances[accountIndex]:N2} OMR");

            // Collect feedback
            CollectFeedback(accountNumber);

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
        public static void AddTransactionWithCurrency(int accountNumber, string type,
    double foreignAmount, double localAmount, double newBalance, string currency)
        {
            transactionAccountNumbers.Add(accountNumber);
            transactionTimestamps.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            transactionTypes.Add(type);
            transactionAmounts.Add(localAmount); // Store converted amount
            transactionBalances.Add(newBalance);

            // Store additional currency information in a separate file
            SaveCurrencyTransaction(accountNumber, type, foreignAmount, currency, localAmount);
        }

        public static void SaveCurrencyTransaction(int accountNumber, string type,
            double foreignAmount, string currency, double localAmount)
        {
            string currencyTransactionsFile = "currency_transactions.txt";
            try
            {
                using (StreamWriter writer = new StreamWriter(currencyTransactionsFile, append: true))
                {
                    writer.WriteLine($"{accountNumber}|{DateTime.Now:yyyy-MM-dd HH:mm:ss}|" +
                                    $"{type}|{foreignAmount}|{currency}|{localAmount}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving currency transaction: {ex.Message}");
            }
        }
        //############################################## withdraw Money #####################################################

        public static void withdrawMoney()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                          Withdraw Page                        \n ");
            Console.WriteLine("**************************************************************\n");
            //  validate user account number

            Console.Write("Enter Account Number: ");
            int accountNumber;
            while (!int.TryParse(Console.ReadLine(), out accountNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid account number.");
            }
            //  validate input withdraw amount

            Console.Write("Enter Amount to Withdraw: ");
            double amount;
            while (!double.TryParse(Console.ReadLine(), out amount) || amount <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive amount.");
            }


            // Check if the account number exists in the system
            if (accountNumbers.Contains(accountNumber))
            {
                // Get the index of the account number to access corresponding balance

                int index = accountNumbers.IndexOf(accountNumber);

                // Check if the account has sufficient funds for the withdrawal

                if (balances[index] >= amount)
                {
                    // Deduct the amount from the balance

                    balances[index] -= amount;
                    // Display success message with new balance
               
                    Console.WriteLine($"Withdrew {amount:C2}. New balance: {balances[index]:C2}");
                    AddTransaction(accountNumber, "WITHDRAWAL", amount, balances[index]);
                    CollectFeedback(accountNumber);
                    Console.ReadKey();
                }
                else
                {
                    // Display error message if not enough funds

                    Console.WriteLine("Insufficient funds.");
                }
            }
            else
            {
                // Display error message if account doesn't exist

                Console.WriteLine("Account not found.");
            }
            SaveUserData();
            // Wait for user to acknowledge before returning to menu
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();

        }

        //############################################## view Balances #####################################################

        public static void viewBalances()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                     VIEW ACCOUNT BALANCE                     ");
            Console.WriteLine("**************************************************************");
            // Check if there are no accounts in the system

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts found in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }
            // Prompt user to enter an account number

            Console.Write("\nEnter account number: ");
            int accountNumber;
            while (!int.TryParse(Console.ReadLine(), out accountNumber))
            {
                Console.Write("Invalid input. Please enter a valid account number: ");
            }
            // Search for the account number in the list

            int index = accountNumbers.IndexOf(accountNumber);

            // Check if the account was found 

            if (index >= 0)
            {
                // Display account info
                Console.WriteLine("\nAccount Details:");
                Console.WriteLine("----------------");
                Console.WriteLine($"Account Number: {accountNumbers[index]}");
                Console.WriteLine($"Account Holder: {accountNames[index]}");
                Console.WriteLine($"Current Balance: {balances[index]:C}");
            }
            else
            {// display message when not found account
                Console.WriteLine($"\nAccount number {accountNumber} not found.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }



        //############################################## View transaction history #####################################################


        public static void viewTransactionHistory()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                   TRANSACTION HISTORY                        ");
            Console.WriteLine("**************************************************************");

            // check if account is exist 
            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts exist in the system.");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }
            // Prompt user to enter an account number

            Console.Write("\nEnter account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }

            // Get the index of the account number to access corresponding balance

            int accountIndex = accountNumbers.IndexOf(accountNumber);

            if (accountIndex == -1)
            {// if account not found
                Console.WriteLine($"Account {accountNumber} not found.");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
                return;
            }
            // display account transaction history
            Console.WriteLine($"\nTransaction History for {accountNames[accountIndex]} (Account: {accountNumber})");
            Console.WriteLine($"Current Balance: {balances[accountIndex]:C2}");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine("{0,-20} {1,-12} {2,12} {3,12}",
                "Date/Time", "Type", "Amount", "Balance");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine("\n{0,-20} {1,-15} {2,12} {3,12} {4,10}",
       "Date/Time", "Type", "Amount", "Balance", "Currency");

            for (int i = 0; i < transactionAccountNumbers.Count; i++)
            {
                string currency = "OMR";
                if (transactionTypes[i].StartsWith("DEPOSIT_"))
                {
                    currency = transactionTypes[i].Split('_')[1];
                }

                Console.WriteLine($"{transactionTimestamps[i],-20} " +
                                 $"{transactionTypes[i],-15} " +
                                 $"{transactionAmounts[i],12:N2} " +
                                 $"{transactionBalances[i],12:N2} " +
                                 $"{currency,10}");
            }
            // Display the transaction details in formatted columns:
            bool foundTransactions = false;
            for (int i = 0; i < transactionAccountNumbers.Count; i++)
            {
                if (transactionAccountNumbers[i] == accountNumber)
                {
                    Console.WriteLine($"{transactionTimestamps[i],-20} {transactionTypes[i],-12} " +
                        $"{transactionAmounts[i],12:C2} {transactionBalances[i],12:C2}");
                    foundTransactions = true;
                }
            }
            // If no transactions were found for this account

            if (!foundTransactions)
            {
                Console.WriteLine("No transactions found for this account.");
            }
            // Wait for user to acknowledge before returning to menu
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        //Records a new transaction and saves it to persistent storage
        public static void AddTransaction(int accountNumber, string type, double amount, double newBalance)
        {
            transactionAccountNumbers.Add(accountNumber);
            transactionTimestamps.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            transactionTypes.Add(type);
            transactionAmounts.Add(amount);
            transactionBalances.Add(newBalance);
            SaveTransactions();
        }



        //############################################## Submit review #####################################################

        public static void submitReview()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                       Submit Review Page                      \n ");
            Console.WriteLine("**************************************************************\n");

            // Prompt user to enter their review text

            Console.Write("Enter your review: ");
            string review = Console.ReadLine();
            // Validate that review contains actual content

            if (!string.IsNullOrEmpty(review))
            {
                //  save reviews to file to prevent data loss

                submittedReviews.Push(review);
                SaveReviews();
                Console.WriteLine("Your review has been submitted.");
            }
            else
            {
                Console.WriteLine("Review cannot be empty.");
            }

            // Wait for user to acknowledge before returning to menu
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();

        }


        //############################################## Admin Interface #####################################################
        public static void AdminIU()
        {
            if (!isAdminAuthenticated)
            {
                Console.WriteLine("Access denied. Please login as admin.");
                Console.WriteLine("\nPress any key to return to main menu...");
                Console.ReadKey();
                return;
            }

            bool keepRunning = true;
            while (keepRunning)
            {
                Console.Clear();
                Console.WriteLine("**************************************************************");
                Console.WriteLine("                      ADMINISTRATOR DASHBOARD                ");
                Console.WriteLine("**************************************************************");

                Console.WriteLine("\nMain Menu Options:");
                Console.WriteLine(" 1. Account Management");
                Console.WriteLine(" 2. Transaction Monitoring");
                Console.WriteLine(" 3. Loan Processing");
                Console.WriteLine(" 4. Appointment Management");
                Console.WriteLine(" 5. Customer Feedback");
                Console.WriteLine(" 6. Financial Reports");
                Console.WriteLine(" 7. System Administration");
                Console.WriteLine(" 0. Logout");

                Console.Write("\nEnter your choice: ");
                if (!int.TryParse(Console.ReadLine(), out int mainChoice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    Console.ReadKey();
                    continue;
                }

                Console.Clear();
                switch (mainChoice)
                {
                    case 1: // Account Management
                        Console.WriteLine("ACCOUNT MANAGEMENT\n");
                        Console.WriteLine("1. View Pending Account Requests");
                        Console.WriteLine("2. Approve New Accounts");
                        Console.WriteLine("3. View All Accounts");
                        Console.WriteLine("4. Search Account by National ID");
                        Console.WriteLine("5. Delete Account");
                        Console.WriteLine("6. Transfer Funds Between Accounts");
                        Console.WriteLine("7. Manage Locked Accounts");
                        Console.WriteLine("8. Update Account Info");
                        Console.WriteLine("0. Back");
                        Console.Write("\nSelect option: ");
                        if (int.TryParse(Console.ReadLine(), out int accChoice))
                        {
                            switch (accChoice)
                            {
                                case 1: ViewAccountRequests(); break;
                                case 2: ProcessNextAccountRequest(); break;
                                case 3: ViewAllAccounts(); break;
                                case 4: SearchByNationalID(); break;
                                case 5: DeleteAccount(); break;
                                case 6: TransferFunds(); break;
                                case 7: ManageLockedAccounts(); break;
                                case 8: UpdateAccountInfo(); break;
                                case 0: break;
                                default: Console.WriteLine("Invalid option."); break;
                            }
                        }
                        break;

                    case 2: // Transaction Monitoring
                        Console.WriteLine("TRANSACTION MONITORING\n");
                        Console.WriteLine("1. View Filtered Transactions");
                        Console.WriteLine("2. View Currency Deposits");
                        Console.WriteLine("3. Print User Transaction History");
                     
                        Console.WriteLine("0. Back");
                        Console.Write("\nSelect option: ");
                        if (int.TryParse(Console.ReadLine(), out int tChoice))
                        {
                            switch (tChoice)
                            {
                                case 1: ViewFilteredTransactions(); break;
                                case 2: ViewCurrencyDeposits(); break;
                                case 3: PrintUserTransactions(); break;
                                case 0: break;
                                default: Console.WriteLine("Invalid option."); break;
                            }
                        }
                        break;

                    case 3: // Loan Processing
                        Console.WriteLine("LOAN PROCESSING\n");
                        Console.WriteLine("1. Process Loan Requests");
                        Console.WriteLine("2. Check Active Loan Status");
                        Console.WriteLine("0. Back");
                        Console.Write("\nSelect option: ");
                        if (int.TryParse(Console.ReadLine(), out int loanChoice))
                        {
                            switch (loanChoice)
                            {
                                case 1:
                                    ProcessLoanRequests();
                                    break;

                                case 2:
                                    Console.Write("Enter account number to check loan status: ");
                                    if (int.TryParse(Console.ReadLine(), out int accountNumber))
                                    {
                                        if (HasActiveLoan(accountNumber))
                                            Console.WriteLine($" Account {accountNumber} has an active loan.");
                                        else
                                            Console.WriteLine($" Account {accountNumber} does not have an active loan.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid account number input.");
                                    }

                                    Console.WriteLine("\nPress any key to return...");
                                    Console.ReadKey();
                                    break;

                                case 0:
                                    break;

                                default:
                                    Console.WriteLine("Invalid option.");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a number.");
                            Console.ReadKey();
                        }
                        break;


                    case 4: // Appointment Management
                        Console.WriteLine("APPOINTMENT MANAGEMENT\n");
                        Console.WriteLine("1. View All Appointments");
                        Console.WriteLine("2. Cancel Appointment");
                        Console.WriteLine("0. Back");
                        Console.Write("\nSelect option: ");
                        if (int.TryParse(Console.ReadLine(), out int aChoice))
                        {
                            switch (aChoice)
                            {
                                case 1: ViewAllAppointments(); break;
                                case 2: CancelAppointment(); break;
                                case 0: break;
                                default: Console.WriteLine("Invalid option."); break;
                            }
                        }
                        break;

                    case 5: // Customer Feedback
                        Console.WriteLine("CUSTOMER FEEDBACK\n");
                        Console.WriteLine("1. View Feedback Stats");
                        Console.WriteLine("2. View Reviews");
                     
                        Console.WriteLine("3. Collect New Feedback");
                        Console.WriteLine("0. Back");
                        Console.Write("\nSelect option: ");
                        if (int.TryParse(Console.ReadLine(), out int fChoice))
                        {
                            switch (fChoice)
                            {
                                case 1: ViewFeedbackStats(); break;
                                case 2: ViewReviews(); break;
                                case 3:
                                    Console.Write("Enter account number for feedback submission: ");
                                    if (int.TryParse(Console.ReadLine(), out int feedbackAccount))
                                    {
                                        CollectFeedback(feedbackAccount);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid account number.");
                                    }
                                    break;
                                case 0: break;
                                default: Console.WriteLine("Invalid option."); break;
                            }
                        }
                        break;

                    case 6: // Financial Reports
                        Console.WriteLine("FINANCIAL REPORTS\n");
                        Console.WriteLine("1. Show Total Bank Balance");
                        Console.WriteLine("2. Show Top 3 Richest Customers");
                        Console.WriteLine("3. Generate Monthly Statement");
                        Console.WriteLine("4. View Currency Exchange Report");
                        Console.WriteLine("0. Back");
                        Console.Write("\nSelect option: ");
                        if (int.TryParse(Console.ReadLine(), out int reportChoice))
                        {
                            switch (reportChoice)
                            {
                                case 1: ShowTotalBankBalance(); break;
                                case 2: ShowTop3RichestCustomers(); break;
                                case 3: GenerateMonthlyStatement(); break;
                                case 4: ViewCurrencyExchangeReport(); break;
                                case 0: break;
                                default: Console.WriteLine("Invalid option."); break;
                            }
                        }
                        break;

                    case 7: // System Administration
                        Console.WriteLine("SYSTEM ADMINISTRATION\n");
                        Console.WriteLine("1. Create Data Backup");
                        Console.WriteLine("2. Change Admin Password");
                       // Console.WriteLine("3. Validate a User Password");
                       // Console.WriteLine("4. Hash a Password");
                       // Console.WriteLine("5. Read Masked Password (for testing)");
                        Console.WriteLine("0. Back");
                        Console.Write("\nSelect option: ");
                        if (int.TryParse(Console.ReadLine(), out int sChoice))
                        {
                            switch (sChoice)
                            {
                                case 1: CreateBackup(); break;
                                case 2: ChangeAdminPassword(); break;
                               // case 3: // Validate a User Password
                                    Console.Write("Enter password to validate: ");
                                    string inputPassword = Console.ReadLine();
                                    if (ValidatePassword(inputPassword))
                                        Console.WriteLine(" Password is valid.");
                                    else
                                        Console.WriteLine("Password is invalid. Must be 8–20 characters, with at least one digit and one letter.");
                                    break;

                                //case 4: // Hash a Password
                                    Console.Write("Enter password to hash: ");
                                    string rawPassword = Console.ReadLine();
                                    string hashed = HashPassword(rawPassword);
                                    Console.WriteLine($" Hashed Password:\n{hashed}");
                                    break;

                                //case 5: ReadMaskedPassword(); break;
                                case 0: break;
                                default: Console.WriteLine("Invalid option."); break;
                            }
                        }
                        break;

                    case 0: // Logout
                        isAdminAuthenticated = false;
                        Console.WriteLine("\nLogged out successfully.");
                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        keepRunning = false;
                        break;

                    default:
                        Console.WriteLine("Invalid main menu option.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // New currency-related methods
        // New currency-related methods
        public static void ViewCurrencyExchangeReport()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                  CURRENCY EXCHANGE REPORT                    ");
            Console.WriteLine("**************************************************************");

            Console.WriteLine("\nCurrent Exchange Rates:");
            Console.WriteLine("{0,-10} {1,-15}", "Currency", "Rate (to OMR)");
            Console.WriteLine(new string('-', 25));

            foreach (var rate in exchangeRates)
            {
                Console.WriteLine("{0,-10} {1,-15:N4}", rate.Key, rate.Value);
            }

            // Show conversion statistics if currency transactions exist
            if (File.Exists("currency_transactions.txt"))
            {
                var currencyStats = new Dictionary<string, int>();
                var currencyAmounts = new Dictionary<string, double>();

                foreach (string line in File.ReadLines("currency_transactions.txt"))
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 6)
                    {
                        string currency = parts[4];
                        double amount = double.Parse(parts[3]); // Foreign amount

                        if (currencyStats.ContainsKey(currency))
                        {
                            currencyStats[currency]++;
                            currencyAmounts[currency] += amount;
                        }
                        else
                        {
                            currencyStats.Add(currency, 1);
                            currencyAmounts.Add(currency, amount);
                        }
                    }
                }

                Console.WriteLine("\nConversion Statistics:");
                Console.WriteLine("{0,-10} {1,-15} {2,-15}", "Currency", "Transactions", "Total Amount");
                Console.WriteLine(new string('-', 40));

                foreach (var stat in currencyStats)
                {
                    Console.WriteLine("{0,-10} {1,-15} {2,-15:N2}",
                        stat.Key, stat.Value, currencyAmounts[stat.Key]);
                }
            }
            else
            {
                Console.WriteLine("\nNo currency exchange transactions found.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        public static void UpdateExchangeRates()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                  UPDATE EXCHANGE RATES                       ");
            Console.WriteLine("**************************************************************");

            Console.WriteLine("\nCurrent Rates:");
            foreach (var rate in exchangeRates)
            {
                Console.WriteLine($"{rate.Key}: {rate.Value:N4} OMR");
            }

            Console.WriteLine("\nSelect currency to update:");
            Console.WriteLine("1. USD");
            Console.WriteLine("2. EUR");
            Console.WriteLine("3. GBP");
            Console.WriteLine("0. Cancel");
            Console.Write("\nEnter choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice > 3)
            {
                Console.WriteLine("\nInvalid selection.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            if (choice == 0) return;

            string currency = choice switch
            {
                1 => "USD",
                2 => "EUR",
                3 => "GBP",
                _ => ""
            };

            Console.Write($"\nEnter new exchange rate for {currency} (1 {currency} = ? OMR): ");
            if (decimal.TryParse(Console.ReadLine(), out decimal newRate) && newRate > 0)
            {
                exchangeRates[currency] = newRate;
                Console.WriteLine($"\n{currency} exchange rate updated to {newRate:N4} OMR");

                // Log this change
                File.AppendAllText("exchange_rate_log.txt",
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|{currency}|{newRate}\n");
            }
            else
            {
                Console.WriteLine("\nInvalid rate. Must be a positive number.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }


        // New appointment management methods
        public static void ViewAllAppointments()
        {
            Console.Clear();
            Console.WriteLine("ALL SCHEDULED APPOINTMENTS\n");
            Console.WriteLine("{0,-15} {1,-25} {2,-25} {3,-15} {4,-10}",
                "Account #", "Customer Name", "Service", "Date", "Time");
            Console.WriteLine(new string('-', 90));

            foreach (string appointment in appointmentQueue)
            {
                string[] parts = appointment.Split('|');
                Console.WriteLine("{0,-15} {1,-25} {2,-25} {3,-15} {4,-10}",
                    parts[0], parts[1], parts[2], parts[3], parts[4]);
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        public static void CancelAppointment()
        {
            Console.Clear();
            Console.WriteLine("CANCEL APPOINTMENT\n");
            ViewAllAppointments();

            Console.Write("\nEnter account number to cancel: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Find and remove appointment
            var updatedQueue = new Queue<string>();
            bool found = false;

            foreach (string appointment in appointmentQueue)
            {
                if (int.Parse(appointment.Split('|')[0]) != accountNumber)
                {
                    updatedQueue.Enqueue(appointment);
                }
                else
                {
                    found = true;
                }
            }

            if (found)
            {
                appointmentQueue = updatedQueue;
                appointmentAccountNumbers.Remove(accountNumber);
                SaveAppointments();
                Console.WriteLine("\nAppointment cancelled successfully.");
            }
            else
            {
                Console.WriteLine("\nNo appointment found for this account.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }


        //############################################## View Account Requests #####################################################

        public static void ViewAccountRequests()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                     Account Requests Page                    \n ");
            Console.WriteLine("**************************************************************\n");
            //  Handle empty state if no accounts exist

            if (requestCreateAccountsInfo.Count == 0)
            {
                Console.WriteLine("No pending account requests.");
            }
            else
            {        // Display account table with column headers and formatting

                Console.WriteLine("Pending Account Requests:");
                Console.WriteLine("Name\tNational ID");

                //  Output each account's details in formatted columns

                foreach (string request in requestCreateAccountsInfo)
                {
                    string[] parts = request.Split('|');
                    if (parts.Length >= 2)
                    {
                        Console.WriteLine($"{parts[0]}\t{parts[1]}");
                    }
                }
            }
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        //############################################## View All Accounts #####################################################

        public static void ViewAllAccounts()
        {
            LoadUserData();

            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                      ALL ACCOUNTS SUMMARY                     ");
            Console.WriteLine("**************************************************************");

            // Debug output
            Console.WriteLine($"\nDebug: Found {accountNumbers.Count} accounts in memory");
            Console.WriteLine($"Debug: Data file exists: {File.Exists(userDataPath)}");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts found in the system.");
                // Additional debug:
                try
                {
                    if (File.Exists(userDataPath))
                    {
                        Console.WriteLine("\nFile content:");
                        Console.WriteLine(File.ReadAllText(userDataPath));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nDebug error reading file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\n{0,-15} {1,-25} {2,10}", "Account Number", "Account Holder", "Balance");
                Console.WriteLine(new string('-', 52));

                for (int i = 0; i < accountNumbers.Count; i++)
                {
                    Console.WriteLine("{0,-15} {1,-25} {2,10:C}",
                        accountNumbers[i],
                        accountNames[i],
                        balances[i]);
                }
                Console.WriteLine("\nTotal Accounts: " + accountNumbers.Count);
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
            Console.Clear();
        }



        //############################################## View Reviews #####################################################

        public static void ViewReviews()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                         Reviews Page                         \n ");
            Console.WriteLine("**************************************************************\n");

            //  Check if there are any reviews to display

            if (submittedReviews.Count == 0)
            {
                Console.WriteLine("No reviews submitted yet.");
            }
            else
            {
                //  Display all reviews with separators

                Console.WriteLine("Latest Reviews:");
                foreach (var review in submittedReviews)
                {
                    Console.WriteLine(review);
                    Console.WriteLine("----------------------");
                }
            }
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }



        //############################################## Process Next Account Request #####################################################

        public static void ProcessNextAccountRequest()
        {
            Console.Clear();
            DisplayProcessingHeader();

            // Check for pending requests
            if (requestCreateAccountsInfo.Count == 0)
            {
                Console.WriteLine("No pending account requests.");
                PauseAndContinue();
                return;
            }

            // Process the next request
            string request = requestCreateAccountsInfo.Dequeue();
            string[] requestData = request.Split('|');

            // Validate request format
            if (requestData.Length < 4) // name|nationalID|phone|address
            {
                Console.WriteLine("Invalid request format. Missing required fields.");
                PauseAndContinue();
                return;
            }

            // Extract request data
            string name = requestData[0];
            string nationalID = requestData[1];
            string phone = requestData[2];
            string address = requestData[3];

            // Generate new account number
            int newAccountNumber = GenerateNewAccountNumber();

            // Create the new account
            CreateAccount(newAccountNumber, name, nationalID, phone, address);
            if (pendingPasswords.Count > 0)
            {
                string approvedPassword = pendingPasswords.Dequeue();
                SavePendingPasswords();
                accountPasswords.Add(approvedPassword);
            }
            else
            {
                accountPasswords.Add(""); // fallback (should never happen ideally)
            }


            // Display success message
            Console.WriteLine($"\nAccount created successfully!");
            Console.WriteLine($"Account Holder: {name}");
            Console.WriteLine($"Account Number: {newAccountNumber}");
            Console.WriteLine($"Initial Balance: {defaultBalances:C}");
            Console.WriteLine("**************************************************************");

            // Save updated data
            SaveUserData();
            PauseAndContinue();
        }

        // Helper Methods

        public static void DisplayProcessingHeader()
        {
            Console.WriteLine("**************************************************************");
            Console.WriteLine("               ACCOUNT REQUEST PROCESSING                     ");
            Console.WriteLine("**************************************************************\n");
        }

        public static int GenerateNewAccountNumber()
        {
            return ++userCordNumber; // Increment and return new account number
        }

        public static void CreateAccount(int accountNumber, string name, string nationalID, string phone, string address)
        {
            // Add to all related collections
            accountNumbers.Add(accountNumber);
            accountNames.Add($"{name}|{nationalID}");
            phoneNumbers.Add(phone);
            addresses.Add(address);
            balances.Add(defaultBalances);

            // Initialize security fields
            failedLoginAttempts.Add(0);
            accountLocked.Add(false);
        }

    


        //############################################## Search By nationa ID #####################################################

        public static void SearchByNationalID()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("               SEARCH ACCOUNT BY NATIONAL ID                  ");
            Console.WriteLine("**************************************************************");

            List<string> validIDs = LoadNationalIDs();
            if (validIDs.Count == 0)
            {
                Console.WriteLine("\nNo national IDs registered in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }
            // user add national id
            Console.Write("\nEnter National ID: ");
            string nationalID = Console.ReadLine().Trim();

            // check for national id in valid
            if (!validIDs.Contains(nationalID))
            {
                Console.WriteLine("\nThis national ID is not registered in our system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }
            var results = accountNames
        .Select((name, index) => new {
            AccountNumber = accountNumbers[index],
            Name = name.Split('|')[0],
            NationalID = name.Split('|').Length > 1 ? name.Split('|')[1] : "",
            Index = index
        })
        .Where(a => a.NationalID.Contains(nationalID, StringComparison.OrdinalIgnoreCase))
        .ToList();
            bool found = false;

            // Search pending requests
            foreach (string request in requestCreateAccountsInfo)
            {
                string[] parts = request.Split('|');
                if (parts.Length >= 2 && parts[1].Trim() == nationalID)
                {
                    Console.WriteLine("\n[PENDING REQUEST FOUND]");
                    Console.WriteLine($"Name: {parts[0]}");
                    Console.WriteLine($"National ID: {parts[1]}");
                    found = true;
                }
            }

            // Search active accounts
            for (int i = 0; i < accountNumbers.Count; i++)
            {
                string[] accountInfo = accountNames[i].Split('|');
                if (accountInfo.Length >= 2 && accountInfo[1].Trim() == nationalID)
                {
                    Console.WriteLine("\n[ACTIVE ACCOUNT FOUND]");
                    Console.WriteLine($"Account Number: {accountNumbers[i]}");
                    Console.WriteLine($"Name: {accountInfo[0]}");
                    Console.WriteLine($"National ID: {accountInfo[1]}");
                    Console.WriteLine($"Balance: {balances[i]:C2}");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("\nNo accounts found with this National ID (though it is registered).");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();

            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }



        //############################################## delete account #####################################################

        public static void DeleteAccount()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                     DELETE ACCOUNT                           ");
            Console.WriteLine("**************************************************************");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts exist in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            Console.Write("\nEnter Account Number to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            int index = accountNumbers.IndexOf(accountNumber);
            if (index == -1)
            {
                Console.WriteLine($"\nAccount {accountNumber} not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get confirmation
            Console.Write($"\nAre you sure you want to delete account {accountNumber}? (Y/N): ");
            string confirmation = Console.ReadLine().ToUpper();

            if (confirmation == "Y")
            {
                // Remove from all parallel lists
                accountNumbers.RemoveAt(index);
                accountNames.RemoveAt(index);
                balances.RemoveAt(index);

                // Remove all transactions for this account
                for (int i = transactionAccountNumbers.Count - 1; i >= 0; i--)
                {
                    if (transactionAccountNumbers[i] == accountNumber)
                    {
                        transactionAccountNumbers.RemoveAt(i);
                        transactionTimestamps.RemoveAt(i);
                        transactionTypes.RemoveAt(i);
                        transactionAmounts.RemoveAt(i);
                        transactionBalances.RemoveAt(i);
                    }
                }

                SaveUserData();
                SaveTransactions();
                Console.WriteLine($"\nAccount {accountNumber} has been deleted successfully.");
            }
            else
            {
                Console.WriteLine("\nAccount deletion cancelled.");
            }



            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }


        //############################################## transfer funds #####################################################

        public static void TransferFunds()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                     TRANSFER FUNDS                           ");
            Console.WriteLine("**************************************************************");

            if (accountNumbers.Count < 2)
            {
                Console.WriteLine("\nThere must be at least two accounts to transfer funds.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get source account
            Console.Write("\nEnter YOUR account number: ");
            if (!int.TryParse(Console.ReadLine(), out int sourceAccount))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            int sourceIndex = accountNumbers.IndexOf(sourceAccount);
            if (sourceIndex == -1)
            {
                Console.WriteLine("\nSource account not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get destination account
            Console.Write("\nEnter RECIPIENT account number: ");
            if (!int.TryParse(Console.ReadLine(), out int destAccount))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            int destIndex = accountNumbers.IndexOf(destAccount);
            if (destIndex == -1)
            {
                Console.WriteLine("\nDestination account not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            if (sourceAccount == destAccount)
            {
                Console.WriteLine("\nCannot transfer to the same account!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get transfer amount
            Console.Write("\nEnter amount to transfer: ");
            if (!double.TryParse(Console.ReadLine(), out double amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Check sufficient balance
            if (balances[sourceIndex] < amount)
            {
                Console.WriteLine("\nInsufficient funds for this transfer.");
                Console.WriteLine($"Current balance: {balances[sourceIndex]:C2}");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Perform transfer
            balances[sourceIndex] -= amount;
            balances[destIndex] += amount;

            // Record transactions
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            AddTransaction(sourceAccount, "TRANSFER_OUT", amount, balances[sourceIndex]);
            AddTransaction(destAccount, "TRANSFER_IN", amount, balances[destIndex]);

            // Save changes
            SaveUserData();

            Console.WriteLine("\nTransfer successful!");
            Console.WriteLine($"\nNew balance for account {sourceAccount}: {balances[sourceIndex]:C2}");
            Console.WriteLine($"New balance for account {destAccount}: {balances[destIndex]:C2}");
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
            Console.Clear();
        }


        //############################################## show top three customers #####################################################

        public static void ShowTop3RichestCustomers()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                  TOP 3 RICHEST CUSTOMERS                     ");
            Console.WriteLine("**************************************************************");
            var topCustomers = accountNumbers
      .Select((num, index) => new {
          AccountNumber = num,
          Name = accountNames[index],
          Balance = balances[index]
      })
      .OrderByDescending(c => c.Balance)
      .Take(3)
      .ToList();
            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts found in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Create a list of indices sorted by balance (descending)
            var sortedIndices = Enumerable.Range(0, accountNumbers.Count)
                .OrderByDescending(i => balances[i])
                .Take(3)
                .ToList();

            Console.WriteLine("\n{0,-5} {1,-15} {2,-25} {3,15}", "Rank", "Account Number", "Customer Name", "Balance");
            Console.WriteLine(new string('-', 62));

            // display top 3 loop for all list than part balance hight to lowst
            for (int i = 0; i < sortedIndices.Count; i++)
            {
                int index = sortedIndices[i];
                string[] accountInfo = accountNames[index].Split('|');
                string customerName = accountInfo.Length > 0 ? accountInfo[0] : "Unknown";

                Console.WriteLine("{0,-5} {1,-15} {2,-25} {3,15:C2}",
                    i + 1,
                    accountNumbers[index],
                    customerName,
                    balances[index]);
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
            Console.Clear();

        }

        //############################################## show total bank balance  #####################################################

        public static void ShowTotalBankBalance()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                   TOTAL BANK BALANCE                         ");
            Console.WriteLine("**************************************************************");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts found in the system.");
            }
            else
            {
                double totalBalance = 0;

                // Calculate sum of all balances
                foreach (double balance in balances)
                {
                    totalBalance += balance;
                }

                Console.WriteLine($"\nTotal Accounts: {accountNumbers.Count}");
                Console.WriteLine($"Total Bank Holdings: {totalBalance:C2}");

                // Optional: Show breakdown by account type if you have different account types
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }



        // Load Reviews from the file
        public static void LoadReviews()
        {
            try
            {
                if (!File.Exists(pathFile)) return;

                submittedReviews.Clear();
                //used to read characters from a stream 
                using (StreamReader reader = new StreamReader(pathFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        submittedReviews.Push(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading reviews: {ex.Message}");
            }
        }
        // save user data
        public static void SaveUserData()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(userDataPath))
                {
                    for (int i = 0; i < accountNumbers.Count; i++)
                    {
                        writer.WriteLine($"{accountNumbers[i]}|{accountNames[i]}|{balances[i]}|" +
                            $"{(accountLockedStatus.Count > i && accountLockedStatus[i])}|" +
                            $"{(failedLoginAttempts.Count > i ? failedLoginAttempts[i] : 0)}|" +
                            $"{(phoneNumbers.Count > i ? phoneNumbers[i] : "")}|" +
                            $"{(addresses.Count > i ? addresses[i] : "")}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user data: {ex.Message}");
            }
        }


        public static void LoadUserData()
        {
            try
            {
                if (!File.Exists(userDataPath)) return;

                accountNumbers.Clear();
                accountNames.Clear();
                balances.Clear();
                accountLockedStatus.Clear();
                failedLoginAttempts.Clear();

                foreach (string line in File.ReadAllLines(userDataPath))
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 3)
                    {
                        accountNumbers.Add(int.Parse(parts[0]));
                        accountNames.Add(parts[1]);
                        balances.Add(double.Parse(parts[2]));

                        // Load locked status (default to false if not specified)
                        accountLockedStatus.Add(parts.Length > 3 && bool.Parse(parts[3]));

                        // Load failed attempts (default to 0 if not specified)
                        failedLoginAttempts.Add(parts.Length > 4 ? int.Parse(parts[4]) : 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user data: {ex.Message}");
            }
        }




        // Load transactions from file
        public static void LoadTransactions()
        {
            try
            {
                if (!File.Exists(transactionsFile)) return;

                string[] lines = File.ReadAllLines(transactionsFile);
                // loop to add transction info to list
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 5)
                    {
                        transactionAccountNumbers.Add(int.Parse(parts[0]));
                        transactionTimestamps.Add(parts[1]);
                        transactionTypes.Add(parts[2]);
                        transactionAmounts.Add(double.Parse(parts[3]));
                        transactionBalances.Add(double.Parse(parts[4]));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading transactions: {ex.Message}");
            }
        }



        // Save the request in the file
        public static void SaveAccountRequest()
        {
            try
            {
                // used to write text data to a file 

                using (StreamWriter writer = new StreamWriter(accountRequestsPath))  // Changed to accountRequestsPath
                {
                    foreach (var request in requestCreateAccountsInfo)
                    {
                        writer.WriteLine(request);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving account requests: {ex.Message}");
            }
        }
        // Load the request from the file
        public static void LoadAccountRequest()
        {
            try
            {
                if (!File.Exists(accountRequestsPath)) return;  // Changed to accountRequestsPath
                requestCreateAccountsInfo.Clear();
                //used to read characters from a stream 

                using (StreamReader reader = new StreamReader(accountRequestsPath))  // Changed to accountRequestsPath
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        requestCreateAccountsInfo.Enqueue(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading account requests: {ex.Message}");
            }
        }





        // load admin info

        public static void LoadAdmi()
        {
            try
            {
                if (!File.Exists(pathAdmin)) return;

                adm.Clear();
                //used to read characters from a stream 

                using (StreamReader reader = new StreamReader(pathAdmin))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (int.TryParse(line, out int adminNumber))
                        {
                            adm.Enqueue(adminNumber);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading admin accounts: {ex.Message}");
            }
        }

        // Save transactions to file
        public static void SaveTransactions()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(transactionsFile))
                {
                    for (int i = 0; i < transactionAccountNumbers.Count; i++)
                    {
                        writer.WriteLine($"{transactionAccountNumbers[i]}|{transactionTimestamps[i]}|" +
                            $"{transactionTypes[i]}|{transactionAmounts[i]}|{transactionBalances[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving transactions: {ex.Message}");
            }
        }
        // Save Reviews to the file
        public static void SaveReviews()
        {
            try
            {
                // used to write text data to a file 
                using (StreamWriter writer = new StreamWriter(pathFile))
                {
                    foreach (var review in submittedReviews)
                    {
                        writer.WriteLine(review);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving reviews: {ex.Message}");
            }
        }





        //############################################## Password Protectionf for accounts  #####################################################
        //gggftgtg
        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public static string ReadMaskedPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace should not work if password is empty
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, (password.Length - 1));
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        public static bool ValidatePassword(string password)
        {
            if (password.Length < MIN_PASSWORD_LENGTH || password.Length > MAX_PASSWORD_LENGTH)
                return false;

            // Check for at least one digit
            if (!password.Any(char.IsDigit))
                return false;

            // Check for at least one letter
            if (!password.Any(char.IsLetter))
                return false;

            return true;
        }

        public static void SavePasswords()
        {
            try
            {
                File.WriteAllLines(passwordPath, accountPasswords);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving passwords: {ex.Message}");
            }
        }

        public static void LoadPasswords()
        {
            try
            {
                if (File.Exists(passwordPath))
                {
                    accountPasswords.Clear();
                    accountPasswords.AddRange(File.ReadAllLines(passwordPath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading passwords: {ex.Message}");
            }
        }

        //##############################################   MONTHLY STATEMENT GENERATOR  #####################################################

        public static void GenerateMonthlyStatement()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                   MONTHLY STATEMENT GENERATOR                ");
            Console.WriteLine("**************************************************************");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts exist in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get account number
            Console.Write("\nEnter your account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            int accountIndex = accountNumbers.IndexOf(accountNumber);
            if (accountIndex == -1)
            {
                Console.WriteLine($"\nAccount {accountNumber} not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get month and year
            Console.Write("\nEnter year (YYYY): ");
            if (!int.TryParse(Console.ReadLine(), out int year) || year < 2000 || year > DateTime.Now.Year)
            {
                Console.WriteLine("Invalid year!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter month (1-12): ");
            if (!int.TryParse(Console.ReadLine(), out int month) || month < 1 || month > 12)
            {
                Console.WriteLine("Invalid month!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Build statement content
            StringBuilder statement = new StringBuilder();
            statement.AppendLine("**************************************************************");
            statement.AppendLine($"            MONTHLY STATEMENT - ACCOUNT {accountNumber}");
            statement.AppendLine($"            {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year}");
            statement.AppendLine("**************************************************************");
            statement.AppendLine($"Account Holder: {accountNames[accountIndex]}");
            statement.AppendLine();
            statement.AppendLine(string.Format("{0,-20} {1,-15} {2,10} {3,10}", "Date", "Type", "Amount", "Balance"));
            statement.AppendLine(new string('-', 60));

            double openingBalance = 0;
            double closingBalance = balances[accountIndex];
            int transactionCount = 0;

            for (int i = 0; i < transactionAccountNumbers.Count; i++)
            {
                if (transactionAccountNumbers[i] == accountNumber)
                {
                    if (DateTime.TryParse(transactionTimestamps[i], out DateTime transactionDate))
                    {
                        if (transactionDate.Year == year && transactionDate.Month == month)
                        {
                            // Record opening balance from first transaction
                            if (transactionCount == 0)
                            {
                                openingBalance = transactionBalances[i] -
                                               (transactionTypes[i] == "DEPOSIT" ? -transactionAmounts[i] : transactionAmounts[i]);
                            }

                            statement.AppendLine(string.Format("{0,-20} {1,-15} {2,10:C2} {3,10:C2}",
                                transactionDate.ToString("yyyy-MM-dd"),
                                transactionTypes[i],
                                transactionAmounts[i],
                                transactionBalances[i]));

                            transactionCount++;
                        }
                    }
                }
            }

            if (transactionCount == 0)
            {
                Console.WriteLine($"\nNo transactions found for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year}.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Add summary
            statement.AppendLine(new string('-', 60));
            statement.AppendLine(string.Format("{0,-35} {1,20:C2}", "Opening Balance:", openingBalance));
            statement.AppendLine(string.Format("{0,-35} {1,20:C2}", "Closing Balance:", closingBalance));
            statement.AppendLine(string.Format("{0,-35} {1,20}", "Number of Transactions:", transactionCount));
            statement.AppendLine("**************************************************************");

            // Display statement
            Console.WriteLine("\n" + statement.ToString());

            // Save to file
            string statementFileName = $"Statement_Acc{accountNumber}_{year}-{month:00}.txt";
            try
            {
                File.WriteAllText(statementFileName, statement.ToString());
                Console.WriteLine($"\nStatement saved to: {statementFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError saving statement: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
            Console.Clear();
        }
        //##############################################   UPDATE ACCOUNT INFO  #####################################################

        public static void UpdateAccountInfo()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                     UPDATE ACCOUNT INFO                      ");
            Console.WriteLine("**************************************************************");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts exist in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get account number
            Console.Write("\nEnter your account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            int accountIndex = accountNumbers.IndexOf(accountNumber);
            if (accountIndex == -1)
            {
                Console.WriteLine($"\nAccount {accountNumber} not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Display current info
            Console.WriteLine("\nCurrent Information:");
            Console.WriteLine($"Name: {accountNames[accountIndex]}");
            Console.WriteLine($"Phone: {phoneNumbers[accountIndex]}");
            Console.WriteLine($"Address: {addresses[accountIndex]}");
            Console.WriteLine("\nWhat would you like to update?");
            Console.WriteLine("1. Phone Number");
            Console.WriteLine("2. Address");
            Console.WriteLine("0. Cancel");

            Console.Write("Enter your choice: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid choice!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            switch (choice)
            {
                case 1:
                    Console.Write("\nEnter new phone number: ");
                    phoneNumbers[accountIndex] = Console.ReadLine();
                    Console.WriteLine("Phone number updated successfully!");
                    break;
                case 2:
                    Console.Write("\nEnter new address: ");
                    addresses[accountIndex] = Console.ReadLine();
                    Console.WriteLine("Address updated successfully!");
                    break;
                case 0:
                    Console.WriteLine("\nUpdate cancelled.");
                    break;
                default:
                    Console.WriteLine("\nInvalid choice!");
                    break;
            }

            // Save changes
            SaveUserData();

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }




        //##############################################   Loan Request Feature       #####################################################

        // Loan request method for users
        public static void RequestLoan()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                        LOAN REQUEST                          ");
            Console.WriteLine("**************************************************************");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts exist in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get account number
            Console.Write("\nEnter your account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            int accountIndex = accountNumbers.IndexOf(accountNumber);
            if (accountIndex == -1)
            {
                Console.WriteLine($"\nAccount {accountNumber} not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Check if user already has an active loan
            if (HasActiveLoan(accountNumber))
            {
                Console.WriteLine("\nYou already have an active loan. Please pay it off before requesting a new one.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Check minimum balance requirement
            if (balances[accountIndex] < MIN_BALANCE_FOR_LOAN)
            {
                Console.WriteLine($"\nMinimum balance of {MIN_BALANCE_FOR_LOAN:C} required to request a loan.");
                Console.WriteLine($"Your current balance: {balances[accountIndex]:C}");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get loan amount
            Console.Write("\nEnter loan amount: ");
            if (!double.TryParse(Console.ReadLine(), out double amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Calculate and show loan terms
            double interest = amount * (DEFAULT_INTEREST_RATE / 100);
            double totalRepayment = amount + interest;

            Console.WriteLine("\nLoan Terms:");
            Console.WriteLine($"Principal Amount: {amount:C}");
            Console.WriteLine($"Interest Rate: {DEFAULT_INTEREST_RATE}%");
            Console.WriteLine($"Total Repayment: {totalRepayment:C}");
            Console.WriteLine($"Duration: 12 months");
            Console.WriteLine($"Monthly Payment: {(totalRepayment / 12):C}");

            Console.Write("\nDo you want to submit this loan request? (Y/N): ");
            if (Console.ReadLine().ToUpper() != "Y")
            {
                Console.WriteLine("\nLoan request cancelled.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Store loan request
            string loanRequest = $"{accountNumber}|{amount}|{DEFAULT_INTEREST_RATE}|{accountNames[accountIndex]}";
            loanRequests.Enqueue(loanRequest);
            SaveLoans();

            Console.WriteLine("\nLoan request submitted for admin approval.");
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // Check for active loans
        public static bool HasActiveLoan(int accountNumber)
        {
            for (int i = 0; i < loanAccountNumbers.Count; i++)
            {
                if (loanAccountNumbers[i] == accountNumber && loanApprovedStatus[i])
                {
                    return true;
                }
            }
            return false;
        }

        // Admin loan approval method
        public static void ProcessLoanRequests()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                   PROCESS LOAN REQUESTS                      ");
            Console.WriteLine("**************************************************************");

            if (loanRequests.Count == 0)
            {
                Console.WriteLine("\nNo pending loan requests.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            string request = loanRequests.Peek();
            string[] parts = request.Split('|');

            int accountNumber = int.Parse(parts[0]);
            double amount = double.Parse(parts[1]);
            double interestRate = double.Parse(parts[2]);
            string accountName = parts[3];

            Console.WriteLine("\nCurrent Loan Request:");
            Console.WriteLine($"Account Number: {accountNumber}");
            Console.WriteLine($"Account Name: {accountName}");
            Console.WriteLine($"Loan Amount: {amount:C}");
            Console.WriteLine($"Interest Rate: {interestRate}%");
            Console.WriteLine($"Total Repayment: {amount * (1 + interestRate / 100):C}");

            Console.WriteLine("\n1. Approve Loan");
            Console.WriteLine("2. Reject Loan");
            Console.WriteLine("0. Back to Menu");
            Console.Write("Enter your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid choice!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            switch (choice)
            {
                case 1: // Approve
                    loanRequests.Dequeue();

                    // Add to approved loans
                    loanAccountNumbers.Add(accountNumber);
                    loanAmounts.Add(amount);
                    interestRates.Add(interestRate);
                    loanApprovedStatus.Add(true);

                    // Credit the loan amount to account
                    int accountIndex = accountNumbers.IndexOf(accountNumber);
                    balances[accountIndex] += amount;

                    // Record transaction
                    AddTransaction(accountNumber, "LOAN", amount, balances[accountIndex]);

                    Console.WriteLine("\nLoan approved and amount credited to account.");
                    break;

                case 2: // Reject
                    loanRequests.Dequeue();
                    Console.WriteLine("\nLoan request rejected.");
                    break;

                case 0:
                    Console.WriteLine("\nNo action taken.");
                    break;

                default:
                    Console.WriteLine("\nInvalid choice!");
                    break;
            }

            SaveLoans();
            SaveUserData();
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        // Save loan data to file
        public static void SaveLoans()
        {
            try
            {
                // Save pending requests
                File.WriteAllLines(loansFilePath + ".requests", loanRequests);

                // Save approved loans
                using (StreamWriter writer = new StreamWriter(loansFilePath + ".approved"))
                {
                    for (int i = 0; i < loanAccountNumbers.Count; i++)
                    {
                        writer.WriteLine($"{loanAccountNumbers[i]}|{loanAmounts[i]}|{interestRates[i]}|{loanApprovedStatus[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving loan data: {ex.Message}");
            }
        }

        // Load loan data from file
        public static void LoadLoans()
        {
            try
            {
                // Load pending requests
                if (File.Exists(loansFilePath + ".requests"))
                {
                    loanRequests = new Queue<string>(File.ReadAllLines(loansFilePath + ".requests"));
                }

                // Load approved loans
                if (File.Exists(loansFilePath + ".approved"))
                {
                    string[] lines = File.ReadAllLines(loansFilePath + ".approved");
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 4)
                        {
                            loanAccountNumbers.Add(int.Parse(parts[0]));
                            loanAmounts.Add(double.Parse(parts[1]));
                            interestRates.Add(double.Parse(parts[2]));
                            loanApprovedStatus.Add(bool.Parse(parts[3]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading loan data: {ex.Message}");
            }
        }




        //##############################################    Transaction Timestamping & Filtering  #####################################################

        public static void ViewFilteredTransactions()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                   TRANSACTION FILTERING                      ");
            Console.WriteLine("**************************************************************");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts exist in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get account number
            Console.Write("\nEnter account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            int accountIndex = accountNumbers.IndexOf(accountNumber);
            if (accountIndex == -1)
            {
                Console.WriteLine($"\nAccount {accountNumber} not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nFilter Options:");
            Console.WriteLine("1. Show last N transactions");
            Console.WriteLine("2. Show transactions after specific date");
            Console.WriteLine("3. Show transactions between dates");
            Console.Write("Enter your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int filterChoice))
            {
                Console.WriteLine("Invalid choice!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }
         
            List<int> transactionIndices = new List<int>();
            string filterDescription = "";

            switch (filterChoice)
            {
                case 1: // Last N transactions
                    Console.Write("\nEnter number of transactions to show: ");
                    if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
                    {
                        Console.WriteLine("Invalid number!");
                        Console.WriteLine("\nPress any key to return to menu...");
                        Console.ReadKey();
                        return;
                    }

                    // Find last N transactions for this account
                    int count = 0;
                    for (int i = transactionAccountNumbers.Count - 1; i >= 0 && count < n; i--)
                    {
                        if (transactionAccountNumbers[i] == accountNumber)
                        {
                            transactionIndices.Add(i);
                            count++;
                        }
                    }
                    transactionIndices.Reverse(); // Show in chronological order
                    filterDescription = $"Last {count} transactions";
                    break;

                case 2: // After specific date
                    Console.Write("\nEnter start date (yyyy-MM-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                    {
                        Console.WriteLine("Invalid date format!");
                        Console.WriteLine("\nPress any key to return to menu...");
                        Console.ReadKey();
                        return;
                    }

                    for (int i = 0; i < transactionAccountNumbers.Count; i++)
                    {
                        if (transactionAccountNumbers[i] == accountNumber &&
                            DateTime.Parse(transactionTimestamps[i]) >= startDate)
                        {
                            transactionIndices.Add(i);
                        }
                    }
                    filterDescription = $"Transactions after {startDate:yyyy-MM-dd}";
                    break;

                case 3: // Between dates
                    Console.Write("\nEnter start date (yyyy-MM-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime fromDate))
                    {
                        Console.WriteLine("Invalid date format!");
                        Console.WriteLine("\nPress any key to return to menu...");
                        Console.ReadKey();
                        return;
                    }

                    Console.Write("Enter end date (yyyy-MM-dd): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime toDate))
                    {
                        Console.WriteLine("Invalid date format!");
                        Console.WriteLine("\nPress any key to return to menu...");
                        Console.ReadKey();
                        return;
                    }

                    for (int i = 0; i < transactionAccountNumbers.Count; i++)
                    {
                        DateTime transDate = DateTime.Parse(transactionTimestamps[i]);
                        if (transactionAccountNumbers[i] == accountNumber &&
                            transDate >= fromDate && transDate <= toDate)
                        {
                            transactionIndices.Add(i);
                        }
                    }
                    filterDescription = $"Transactions between {fromDate:yyyy-MM-dd} and {toDate:yyyy-MM-dd}";
                    break;

                default:
                    Console.WriteLine("Invalid choice!");
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey();
                    return;
            }
            // LINQ implementation for filtering
            var filteredTransactions = transactionAccountNumbers
                .Select((num, index) => new {
                    AccountNumber = num,
                    Index = index,
                    Timestamp = transactionTimestamps[index],
                    Type = transactionTypes[index],
                    Amount = transactionAmounts[index],
                    Balance = transactionBalances[index]
                })
                .Where(t => t.AccountNumber == accountNumber)
                .OrderByDescending(t => DateTime.Parse(t.Timestamp))
                .Take(3) // For "last N transactions" filter
                .ToList();
            // Display filtered transactions
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine($"           TRANSACTION HISTORY - ACCOUNT {accountNumber}      ");
            Console.WriteLine($"               {filterDescription}                           ");
            Console.WriteLine("**************************************************************");
            Console.WriteLine($"Account Holder: {accountNames[accountIndex]}");
            Console.WriteLine($"Current Balance: {balances[accountIndex]:C2}");
            Console.WriteLine();
            Console.WriteLine("{0,-20} {1,-12} {2,12} {3,12}", "Date/Time", "Type", "Amount", "Balance");
            Console.WriteLine(new string('-', 60));

            if (transactionIndices.Count == 0)
            {
                Console.WriteLine("No transactions found matching your criteria.");
            }
            else
            {
                foreach (int i in transactionIndices)
                {
                    Console.WriteLine($"{transactionTimestamps[i],-20} {transactionTypes[i],-12} " +
                        $"{transactionAmounts[i],12:C2} {transactionBalances[i],12:C2}");
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }




        //##############################################   . Admin Authentication        #####################################################

        
        public static void ChangeAdminPassword()
        {
            if (!isAdminAuthenticated)
            {
                Console.WriteLine("Access denied. Admin authentication required.");
                return;
            }

            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                     CHANGE ADMIN PASSWORD                    ");
            Console.WriteLine("**************************************************************");

            Console.Write("\nEnter current password: ");
            string currentPassword = ReadMaskedPassword();

            if (currentPassword != ADMIN_PASSWORD)
            {
                Console.WriteLine("\nIncorrect current password.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            Console.Write("\nEnter new password: ");
            string newPassword = ReadMaskedPassword();

            Console.Write("Confirm new password: ");
            string confirmPassword = ReadMaskedPassword();

            if (newPassword != confirmPassword)
            {
                Console.WriteLine("\nPasswords do not match.");
            }
            else if (newPassword == ADMIN_PASSWORD)
            {
                Console.WriteLine("\nNew password must be different from current password.");
            }
            else
            {
                // In a real application, you would update the stored password here
                // For this example, we're using a const so it can't be changed at runtime
                Console.WriteLine("\nPassword changed successfully! (Note: In this demo, the change isn't persistent)");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        //##############################################   User Feedback System   #####################################################
        public static void CollectFeedback(int accountNumber)
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                      SERVICE FEEDBACK                        ");
            Console.WriteLine("**************************************************************");

            Console.WriteLine("\nHow would you rate your experience with this transaction?");
            Console.WriteLine("1 - Very Poor");
            Console.WriteLine("2 - Poor");
            Console.WriteLine("3 - Average");
            Console.WriteLine("4 - Good");
            Console.WriteLine("5 - Excellent");

            int rating;
            do
            {
                Console.Write("\nEnter your rating (1-5): ");
            } while (!int.TryParse(Console.ReadLine(), out rating) || rating < 1 || rating > 5);

            Console.Write("\nOptional comments (press Enter to skip): ");
            string comment = Console.ReadLine();

            // Store feedback
            feedbackScores.Add(rating);
            feedbackAccountNumbers.Add(accountNumber);
            feedbackComments.Add(comment ?? string.Empty);
            SaveFeedback();

            Console.WriteLine("\nThank you for your feedback!");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public static void SaveFeedback()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(feedbackFilePath, append: true))
                {
                    int lastIndex = feedbackScores.Count - 1;
                    writer.WriteLine($"{feedbackAccountNumbers[lastIndex]}|{feedbackScores[lastIndex]}|{feedbackComments[lastIndex]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving feedback: {ex.Message}");
            }
        }

        public static void LoadFeedback()
        {
            try
            {
                if (!File.Exists(feedbackFilePath)) return;

                feedbackScores.Clear();
                feedbackAccountNumbers.Clear();
                feedbackComments.Clear();

                foreach (string line in File.ReadAllLines(feedbackFilePath))
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 2)
                    {
                        feedbackAccountNumbers.Add(int.Parse(parts[0]));
                        feedbackScores.Add(int.Parse(parts[1]));
                        feedbackComments.Add(parts.Length > 2 ? parts[2] : string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading feedback: {ex.Message}");
            }
        }

        public static void ViewFeedbackStats()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                   FEEDBACK STATISTICS                        ");
            Console.WriteLine("**************************************************************");

            if (feedbackScores.Count == 0)
            {
                Console.WriteLine("\nNo feedback received yet.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Calculate statistics
            double averageRating = feedbackScores.Average();
            int totalResponses = feedbackScores.Count;
            int[] ratingCounts = new int[5];
            foreach (int score in feedbackScores)
            {
                ratingCounts[score - 1]++;
            }

            Console.WriteLine("\nOverall Feedback Summary:");
            Console.WriteLine($"Average Rating: {averageRating:F1}/5");
            Console.WriteLine($"Total Responses: {totalResponses}");
            Console.WriteLine("\nRating Distribution:");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"{i + 1} star{(i == 0 ? "" : "s")}: {ratingCounts[i]} responses");
            }

            Console.WriteLine("\nRecent Comments:");
            int commentsToShow = Math.Min(5, feedbackComments.Count);
            for (int i = feedbackComments.Count - 1; i >= Math.Max(0, feedbackComments.Count - 5); i--)
            {
                if (!string.IsNullOrEmpty(feedbackComments[i]))
                {
                    Console.WriteLine($"- {feedbackComments[i]}");
                    Console.WriteLine($"  (Account: {feedbackAccountNumbers[i]}, Rating: {feedbackScores[i]}/5)");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        //##############################################  . Data Backup System    #####################################################
        public static void CreateBackup()
        {
            try
            {
                Console.Write("Would you like to save a backup of all data? (y/n): ");
                string input = Console.ReadLine().ToLower();
                if (input != "y") return;

                string backupDir = Path.Combine(Directory.GetCurrentDirectory(), "Backups");
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmm");
                string backupFileName = $"Backup_{timestamp}.txt";
                string backupPath = Path.Combine(backupDir, backupFileName);

                StringBuilder backupData = new StringBuilder();
                backupData.AppendLine("==== Mini Bank System Backup ====");
                backupData.AppendLine($"Backup Date: {DateTime.Now}\n");

                // --- USER ACCOUNTS ---
                backupData.AppendLine("[USER ACCOUNTS]");
                int accountCount = new[] { accountNumbers.Count, accountNames.Count, balances.Count, phoneNumbers.Count, addresses.Count }.Min();
                for (int i = 0; i < accountCount; i++)
                {
                    backupData.AppendLine($"{accountNumbers[i]}|{accountNames[i]}|{balances[i]}|{phoneNumbers[i]}|{addresses[i]}");
                }

                // --- TRANSACTIONS ---
                backupData.AppendLine("\n[TRANSACTIONS]");
                int transactionCount = new[] { transactionAccountNumbers.Count, transactionTimestamps.Count, transactionTypes.Count, transactionAmounts.Count, transactionBalances.Count }.Min();
                for (int i = 0; i < transactionCount; i++)
                {
                    backupData.AppendLine($"{transactionAccountNumbers[i]}|{transactionTimestamps[i]}|{transactionTypes[i]}|{transactionAmounts[i]}|{transactionBalances[i]}");
                }

                // --- LOANS ---
                backupData.AppendLine("\n[LOANS]");
                int loanCount = new[] { loanAccountNumbers.Count, loanAmounts.Count, interestRates.Count, loanApprovedStatus.Count }.Min();
                for (int i = 0; i < loanCount; i++)
                {
                    backupData.AppendLine($"{loanAccountNumbers[i]}|{loanAmounts[i]}|{interestRates[i]}|{loanApprovedStatus[i]}");
                }

                // --- FEEDBACK ---
                backupData.AppendLine("\n[FEEDBACK]");
                int feedbackCount = new[] { feedbackAccountNumbers.Count, feedbackScores.Count, feedbackComments.Count }.Min();
                for (int i = 0; i < feedbackCount; i++)
                {
                    backupData.AppendLine($"{feedbackAccountNumbers[i]}|{feedbackScores[i]}|{feedbackComments[i]}");
                }

                // Write file
                File.WriteAllText(backupPath, backupData.ToString());

                Console.WriteLine($"\n Backup successfully created: {backupFileName}");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Error creating backup: {ex.Message}");
            }
        }


        //##############################################   Print All Transactions of a User  #####################################################
        public static void PrintUserTransactions()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                   USER TRANSACTION HISTORY                   ");
            Console.WriteLine("**************************************************************");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts exist in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get account number
            Console.Write("\nEnter account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Verify account exists
            int accountIndex = accountNumbers.IndexOf(accountNumber);
            if (accountIndex == -1)
            {
                Console.WriteLine($"\nAccount {accountNumber} not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Check if user is admin or the account owner
            bool isAccountOwner = (isAdminAuthenticated ||
                                  (accountNumbers.Contains(accountNumber) &&
                                   accountNumbers.IndexOf(accountNumber) == accountIndex));

            if (!isAccountOwner)
            {
                Console.WriteLine("\nAccess denied. You can only view your own transactions.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get all transactions for this account
            var userTransactions = new List<int>();
            for (int i = 0; i < transactionAccountNumbers.Count; i++)
            {
                if (transactionAccountNumbers[i] == accountNumber)
                {
                    userTransactions.Add(i);
                }
            }

            if (userTransactions.Count == 0)
            {
                Console.WriteLine($"\nNo transactions found for account {accountNumber}.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Display transaction history
            Console.WriteLine($"\nTransaction History for: {accountNames[accountIndex]} (Account: {accountNumber})");
            Console.WriteLine($"Current Balance: {balances[accountIndex]:C2}");
            Console.WriteLine(new string('-', 85));
            Console.WriteLine("{0,-20} {1,-15} {2,15} {3,15} {4,15}",
                "Date/Time", "Type", "Amount", "Balance After", "Description");
            Console.WriteLine(new string('-', 85));

            foreach (int index in userTransactions)
            {
                string description = GetTransactionDescription(transactionTypes[index]);
                Console.WriteLine("{0,-20} {1,-15} {2,15:C2} {3,15:C2} {4,15}",
                    transactionTimestamps[index],
                    transactionTypes[index],
                    transactionAmounts[index],
                    transactionBalances[index],
                    description);
            }

            Console.WriteLine(new string('-', 85));
            Console.WriteLine($"\nTotal Transactions: {userTransactions.Count}");
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        public static string GetTransactionDescription(string transactionType)
        {
            switch (transactionType.ToUpper())
            {
                case "DEPOSIT":
                    return "Cash Deposit";
                case "WITHDRAWAL":
                    return "ATM Withdrawal";
                case "TRANSFER_IN":
                    return "Funds Received";
                case "TRANSFER_OUT":
                    return "Funds Sent";
                case "LOAN":
                    return "Loan Disbursement";
                default:
                    return "Other Transaction";
            }
        }
        //##############################################    Appointment Booking for Bank Services (Creative Add-on)  #####################################################
        public static void BookAppointment()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                  BOOK BANK APPOINTMENT                       ");
            Console.WriteLine("**************************************************************");

            if (accountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo accounts exist in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Get account number
            Console.Write("\nEnter your account number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            int accountIndex = accountNumbers.IndexOf(accountNumber);
            if (accountIndex == -1)
            {
                Console.WriteLine($"\nAccount {accountNumber} not found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Check for existing appointment
            if (appointmentAccountNumbers.Contains(accountNumber))
            {
                Console.WriteLine("\nYou already have a pending appointment.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Display available services
            Console.WriteLine("\nAvailable Services:");
            Console.WriteLine("1. Loan Consultation");
            Console.WriteLine("2. Account Opening");
            Console.WriteLine("3. Investment Advice");
            Console.WriteLine("4. Mortgage Discussion");
            Console.WriteLine("5. Other Banking Services");

            Console.Write("\nSelect service (1-5): ");
            if (!int.TryParse(Console.ReadLine(), out int serviceChoice) || serviceChoice < 1 || serviceChoice > 5)
            {
                Console.WriteLine("Invalid service choice!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            string service = serviceChoice switch
            {
                1 => "Loan Consultation",
                2 => "Account Opening",
                3 => "Investment Advice",
                4 => "Mortgage Discussion",
                _ => "Other Banking Services"
            };

            // Date selection
            Console.WriteLine($"\nAvailable dates (next {MAX_DAYS_IN_ADVANCE} days):");
            DateTime today = DateTime.Today;
            for (int i = 1; i <= MAX_DAYS_IN_ADVANCE; i++)
            {
                DateTime date = today.AddDays(i);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    Console.WriteLine($"{i}. {date:ddd, MMM dd yyyy}");
                }
            }

            Console.Write("\nSelect day (1-30): ");
            if (!int.TryParse(Console.ReadLine(), out int dayChoice) || dayChoice < 1 || dayChoice > MAX_DAYS_IN_ADVANCE)
            {
                Console.WriteLine("Invalid day selection!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            DateTime appointmentDate = today.AddDays(dayChoice);

            // Time selection
            Console.WriteLine("\nAvailable Time Slots:");
            string[] timeSlots = { "09:00 AM", "10:30 AM", "02:00 PM", "03:30 PM" };
            for (int i = 0; i < timeSlots.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {timeSlots[i]}");
            }

            Console.Write("\nSelect time slot (1-4): ");
            if (!int.TryParse(Console.ReadLine(), out int timeChoice) || timeChoice < 1 || timeChoice > 4)
            {
                Console.WriteLine("Invalid time selection!");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Create appointment record
            string appointmentTime = timeSlots[timeChoice - 1];
            string appointmentRecord = $"{accountNumber}|{accountNames[accountIndex]}|{service}|{appointmentDate:yyyy-MM-dd}|{appointmentTime}";

            appointmentQueue.Enqueue(appointmentRecord);
            appointmentAccountNumbers.Add(accountNumber);
            SaveAppointments();

            Console.WriteLine("\nAppointment Booked Successfully!");
            Console.WriteLine($"\nService: {service}");
            Console.WriteLine($"Date: {appointmentDate:ddd, MMM dd yyyy}");
            Console.WriteLine($"Time: {appointmentTime}");
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        public static void ViewAppointments()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                  MANAGE APPOINTMENTS                         ");
            Console.WriteLine("**************************************************************");

            if (appointmentQueue.Count == 0)
            {
                Console.WriteLine("\nNo appointments scheduled.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n{0,-15} {1,-25} {2,-25} {3,-15} {4,-10}",
                "Account #", "Customer Name", "Service", "Date", "Time");
            Console.WriteLine(new string('-', 90));

            foreach (string appointment in appointmentQueue)
            {
                string[] parts = appointment.Split('|');
                Console.WriteLine("{0,-15} {1,-25} {2,-25} {3,-15} {4,-10}",
                    parts[0], parts[1], parts[2], parts[3], parts[4]);
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }

        public static void SaveAppointments()
        {
            try
            {
                File.WriteAllLines(appointmentsFilePath, appointmentQueue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving appointments: {ex.Message}");
            }
        }

        public static void LoadAppointments()
        {
            try
            {
                if (File.Exists(appointmentsFilePath))
                {
                    appointmentQueue = new Queue<string>(File.ReadAllLines(appointmentsFilePath));
                    appointmentAccountNumbers = appointmentQueue
                        .Select(a => int.Parse(a.Split('|')[0]))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading appointments: {ex.Message}");
            }
        }

        //##############################################    Currency Converter Before Deposit  #####################################################

        public static void ViewCurrencyDeposits()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                  FOREIGN CURRENCY DEPOSITS                   ");
            Console.WriteLine("**************************************************************");

            if (!File.Exists("currency_transactions.txt"))
            {
                Console.WriteLine("\nNo foreign currency transactions found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n{0,-15} {1,-20} {2,-15} {3,12} {4,10} {5,12}",
                "Account", "Date/Time", "Type", "F.Amount", "Currency", "L.Amount");
            Console.WriteLine(new string('-', 85));

            foreach (string line in File.ReadLines("currency_transactions.txt"))
            {
                string[] parts = line.Split('|');
                if (parts.Length == 6)
                {
                    Console.WriteLine("{0,-15} {1,-20} {2,-15} {3,12:N2} {4,10} {5,12:N2}",
                        parts[0], parts[1], parts[2],
                        double.Parse(parts[3]), parts[4],
                        double.Parse(parts[5]));
                }
            }

            // Calculate totals
            var currencyTotals = new Dictionary<string, double>();
            foreach (string line in File.ReadLines("currency_transactions.txt"))
            {
                string[] parts = line.Split('|');
                if (parts.Length == 6)
                {
                    string currency = parts[4];
                    double amount = double.Parse(parts[5]); // Local amount

                    if (currencyTotals.ContainsKey(currency))
                        currencyTotals[currency] += amount;
                    else
                        currencyTotals.Add(currency, amount);
                }
            }

            Console.WriteLine("\nTotal Deposits by Currency:");
            foreach (var pair in currencyTotals)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value:N2} OMR");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
        //##############################################    Lock Account After 3 Failed Logins  #####################################################

        public static void logeInSystem()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************\n");
            Console.WriteLine("                          LOGIN PAGE                           \n ");
            Console.WriteLine("**************************************************************\n");

            Console.Write("Enter Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number format.");
                Console.WriteLine("\nPress any key to return to main menu...");
                Console.ReadKey();
                return;
            }

            // Check if account exists
            int accountIndex = accountNumbers.IndexOf(accountNumber);
            if (accountIndex == -1)
            {
                Console.WriteLine("\nAccount not found.");
                Console.Write("Would you like to create a new account? (Y/N): ");
                string response = Console.ReadLine()?.ToUpper();

                if (response == "Y")
                {
                    requestCreateAccounts();
                }
                else
                {
                    Console.WriteLine("\nReturning to main menu...");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
                return;
            }

            // Check if account is locked
            if (accountLockedStatus.Count > accountIndex && accountLockedStatus[accountIndex])
            {
                Console.WriteLine("\nAccount locked. Please contact administrator.");
                Console.WriteLine("\nPress any key to return to main menu...");
                Console.ReadKey();
                return;
            }

            // Password verification
            Console.Write("Enter Password: ");
            string password = ReadMaskedPassword();
            string hashedPassword = HashPassword(password);

            if (accountPasswords[accountIndex] == hashedPassword)
            {
                // Reset failed attempts on successful login
                if (failedLoginAttempts.Count > accountIndex)
                {
                    failedLoginAttempts[accountIndex] = 0;
                }
                Console.WriteLine("\nLogin successful!");
                Console.WriteLine("\nPress any key to continue to user menu...");
                Console.ReadKey();
                UserIU();
            }
            else
            {
                // Increment failed attempts
                if (failedLoginAttempts.Count <= accountIndex)
                {
                    failedLoginAttempts.Add(0);
                }
                failedLoginAttempts[accountIndex]++;

                Console.WriteLine($"\nInvalid password. Attempts remaining: {MAX_LOGIN_ATTEMPTS - failedLoginAttempts[accountIndex]}");

                // Lock account if max attempts reached
                if (failedLoginAttempts[accountIndex] >= MAX_LOGIN_ATTEMPTS)
                {
                    if (accountLockedStatus.Count <= accountIndex)
                    {
                        accountLockedStatus.Add(false);
                    }
                    accountLockedStatus[accountIndex] = true;
                    Console.WriteLine("\nAccount locked due to too many failed attempts. Contact administrator.");
                    SaveUserData(); // Persist the locked status
                }

                Console.WriteLine("\nPress any key to return to main menu...");
                Console.ReadKey();
            }
        }


        public static void ManageLockedAccounts()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                  MANAGE LOCKED ACCOUNTS                      ");
            Console.WriteLine("**************************************************************");

            var lockedAccounts = accountNumbers
                .Select((num, index) => new { Number = num, Index = index })
                .Where(x => accountLockedStatus.Count > x.Index && accountLockedStatus[x.Index])
                .ToList();

            if (lockedAccounts.Count == 0)
            {
                Console.WriteLine("\nNo locked accounts found.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nLocked Accounts:");
            Console.WriteLine("{0,-15} {1,-25} {2,-20}", "Account #", "Customer Name", "Locked Since");
            Console.WriteLine(new string('-', 60));

            foreach (var account in lockedAccounts)
            {
                string name = accountNames[account.Index];
                Console.WriteLine("{0,-15} {1,-25} {2,-20}",
                    account.Number, name, "N/A"); // Optionally add a lock timestamp later
            }

            Console.Write("\nEnter account number to unlock (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber) || accountNumber == 0)
            {
                Console.WriteLine("\nOperation cancelled.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            var accountToUnlock = lockedAccounts.FirstOrDefault(a => a.Number == accountNumber);
            if (accountToUnlock != null)
            {
                accountLockedStatus[accountToUnlock.Index] = false;
                failedLoginAttempts[accountToUnlock.Index] = 0;
                SaveUserData();
                Console.WriteLine($"\n✅ Account {accountNumber} unlocked successfully.");
            }
            else
            {
                Console.WriteLine("\n❌ Account not found or not locked.");
            }

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }



        public static void ViewAllTransactions()
        {
            Console.Clear();
            Console.WriteLine("**************************************************************");
            Console.WriteLine("                   ALL TRANSACTIONS REPORT                     ");
            Console.WriteLine("**************************************************************");

            if (transactionAccountNumbers.Count == 0)
            {
                Console.WriteLine("\nNo transactions found in the system.");
                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                return;
            }

            // Using LINQ to join transactions with account names
            var transactionDetails = transactionAccountNumbers
                .Select((accountNum, index) => new {
                    AccountNumber = accountNum,
                    AccountName = accountNames.ElementAtOrDefault(accountNumbers.IndexOf(accountNum)) ?? "Unknown",
                    Timestamp = transactionTimestamps[index],
                    Type = transactionTypes[index],
                    Amount = transactionAmounts[index],
                    Balance = transactionBalances[index]
                })
                .OrderByDescending(t => DateTime.Parse(t.Timestamp));

            Console.WriteLine("\n{0,-15} {1,-25} {2,-20} {3,-12} {4,12} {5,12}",
                "Account #", "Account Name", "Date/Time", "Type", "Amount", "Balance");
            Console.WriteLine(new string('-', 100));

            foreach (var trans in transactionDetails)
            {
                Console.WriteLine("{0,-15} {1,-25} {2,-20} {3,-12} {4,12:C2} {5,12:C2}",
                    trans.AccountNumber,
                    trans.AccountName,
                    trans.Timestamp,
                    trans.Type,
                    trans.Amount,
                    trans.Balance);
            }

            // Summary using LINQ
            Console.WriteLine("\nTransaction Summary:");
            Console.WriteLine("Total Transactions: " + transactionDetails.Count());
            Console.WriteLine("Total Deposit Amount: " +
                transactionDetails.Where(t => t.Type == "DEPOSIT").Sum(t => t.Amount).ToString("C2"));
            Console.WriteLine("Total Withdrawal Amount: " +
                transactionDetails.Where(t => t.Type == "WITHDRAWAL").Sum(t => t.Amount).ToString("C2"));

            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }



        
    }
}

