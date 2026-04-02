# Digital Wallet & Rewards/Loyalty System

## Step 1: SharedContracts Library
- [x] Create solution structure (`DigitalWallet.sln`, folder layout)
- [x] Create `SharedContracts` class library project
- [x] Define all RabbitMQ event message classes
- [x] Define shared DTOs and enums

## Step 2: AuthService
- [x] AuthService project setup (NuGet packages, Program.cs)
- [x] Domain entities & enums
- [x] Infrastructure: DbContext, repositories
- [x] Application: DTOs, interfaces, services (Auth, OTP, KYC)
- [x] Controllers (AuthController, KYCController, OTPController)
- [x] Middleware (global exception handler)
- [x] RabbitMQ event publishing via MassTransit

## Step 3: Ocelot Gateway
- [x] Gateway project with Ocelot config, JWT validation, rate limiting

## Step 4: WalletService
- [x] Project setup (NuGet packages, references)
- [x] Domain entities (WalletAccount, LedgerEntry, TopUpRequest, TransferRequest, DailyLimitTracker)
- [x] Infrastructure: DbContext, repositories
- [x] Application: DTOs, interfaces, services (balance, topup, transfer, transactions)
- [x] MassTransit consumers (UserRegistered, KYCApproved)
- [x] Controllers (WalletController)
- [x] Middleware + Program.cs wiring

## Step 5: RewardsService
- [x] Project setup (NuGet packages, references)
- [x] Domain entities (RewardsAccount, RewardsTransaction, EarnRule, RewardsCatalogItem, Redemption)
- [x] Infrastructure: DbContext
- [x] Application: DTOs, interfaces, services (rewards, redemption)
- [x] MassTransit consumers (UserRegistered, TopUpCompleted, TransferCompleted)
- [x] Controllers (RewardsController)
- [x] Middleware + Program.cs wiring

## Step 6: NotificationService (on user approval)
- [x] Event consumers, templates, logging

## Step 7: AdminService (on user approval)
- [x] KYC management, campaigns, dashboard

## Step 8: Docker Compose (on user approval)
- [x] docker-compose.yml, Dockerfiles, networking
