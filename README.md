# 🏦 Mini Bank System – Full Console-Based C# Banking Project

A powerful and feature-rich bank management system built in C# without classes. This project uses core data structures like lists, stacks, and queues to simulate real-world banking functionality in a console interface. The system is designed for both **customers** and **admins**, covering everything from login and balance checks to complaints, loan requests, and admin-level control.

---

## 📋 Table of Contents

- [Features](#features)
- [User Functionalities](#user-functionalities)
- [Admin Functionalities](#admin-functionalities)
- [Data Storage](#data-storage)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
- [Screenshots](#screenshots)
- [To-Do / Enhancements](#to-do--enhancements)
- [Contributing](#contributing)
- [License](#license)

---

## ✅ Features

- Admin and User authentication with password protection
- Account locking after 3 wrong attempts
- File-based persistent storage with optional backup
- Deposit in OMR, USD, EUR with auto conversion
- Detailed transactions history and monthly statements
- Use of LINQ for sorting, filtering, and searching
- Beautiful console design with colors and ASCII UI

---

## 👤 User Functionalities

- Login and signup
- Deposit (with currency conversion)
- Withdraw
- Transfer funds
- View transaction history
- Generate monthly statements
- Request loans
- Submit and undo complaints
- Book appointments
- View feedback summary

---

## 🛠 Admin Functionalities

- Admin login with hashed password
- View and process account creation requests (approve/reject)
- Search user accounts by name or national ID
- View all accounts sorted by balance
- View complaints and feedback
- Unlock locked accounts
- Process pending loan requests
- View scheduled appointments
- Show top 3 richest customers
- Export account data

---

## 💾 Data Storage

Stored as plain `.txt` files for simplicity and transparency:
- `accounts.txt`
- `transactions.txt`
- `appointments.txt`
- `complaints.txt`
- `transferHistory.txt`

Each file is parsed and updated on runtime. Backup option available on exit.

---

## 🧠 Technology Stack

- **Language**: C# (no classes used)
- **Platform**: .NET Console Application
- **Data Structures**: Lists, Stacks, Queues
- **Utilities**: LINQ, File I/O, ASCII UI

---

## 🚀 Getting Started

```bash
git clone https://github.com/your-username/mini-bank-system.git
cd mini-bank-system
open Program.cs in Visual Studio
run the application
