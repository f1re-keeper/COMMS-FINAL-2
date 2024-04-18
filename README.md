# COMMS-FINAL-2
This project implements a Web API for managing users' loans and also how users interact with their loans and how accountants interact with users
# Controllers
Accountant Controller:
Allows accountants to view, update, and delete loans. Accountants can also block users for a certain period, prohibiting them from making any loans.

User Controller:
Supports user registration and authentication. Allows users to view their information based on their ID. Users can request loans depending on their status (isBlocked).

Loan Entity:
Defines the fields for loan requests, including type, amount, currency, period, and status.

Authorization:
Implements role-based authorization, accountants have some accountant-specific functions. Users have limited rights regarding loan-taking and viewing their information.

Database Setup:
Describes the setup process for the database.
