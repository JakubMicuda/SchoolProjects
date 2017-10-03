/**
 *@author Jakub Micuda(433715)
 */
#ifndef INTERACTIVE_H
#define INTERACTIVE_H
#include "BankRegister.h"
#include "Account.h"
#include "Customer.h"
#include "Bank.h"

#include <iostream>
#include <sstream>
#include <string>

/**
 * @brief Enum for possible commands
 */
enum Command {
    CREATEBANK,
    CREATECLIENT,
    CREATEACCOUNT,
    DEPOSIT,
    WITHDRAW,
    TRANSFER,
    BANKINFO,
    CUSTOMERINFO,
    ACCOUNTINFO,
    QUIT,
    NONE
};

typedef unsigned int uint;

/**
 * @brief Class for easier manipulating of input
 */
class Interactive
{
private:
    BankRegister bankRegister;
    std::vector<uint> banks;
public:

    /**
     * @brief creates new Bank
     * @param name  banks name
     */
    void createBank(std::string const & name);

    /**
     * @brief creates new Client
     * @param bankNumber    banks ID
     * @param name          clients name
     */
    void createClient(uint bankNumber, std::string const & name);

    /**
     * @brief creates new Account
     * @param bankNumber    banks ID
     * @param ownerNumber   customers ID
     */
    void createAccount(uint bankNumber, uint ownerNumber);

    /**
     * @brief deposits money to target account
     * @param target    AccountID of target
     * @param amount    amount of money
     */
    void deposit(AccountID target, uint amount);

    /**
     * @brief withdraws money from target account
     * @param target    AccountID of target
     * @param amount    amount of money
     */
    void withdraw(AccountID target, uint amount);

    /**
     * @brief transfers money form source account to target account
     * @param source    AccountID of source
     * @param target    AccountID of target
     * @param amount    amount of money
     */
    void transfer(AccountID source, AccountID target, uint amount);

    /**
     * @brief prints info about bank
     * @param bankNumber    banks ID
     */
    void bankInfo(uint bankNumber);

    /**
     * @brief prints info about customer
     * @param bankNumber    banks ID
     * @param customerID    customers ID
     */
    void customerInfo(uint bankNumber, uint customerID);

    /**
     * @brief prints info about account
     * @param target    AccountID of account
     */
    void accountInfo(AccountID target);

    /**
     * @brief clears register
     */
    void clearRegister();
};

/******************************OVERLOADED OPERATORS********************************/

std::istream & operator>>(std::istream & in, AccountID & accountID);

std::ostream & operator<<(std::ostream & out, const AccountID & accountID);
std::ostream & operator<<(std::ostream & out, Bank & bank);
std::ostream & operator<<(std::ostream & out, CustomerInterface & customer);
std::ostream & operator<<(std::ostream & out, AccountInterface & account);
std::ostream & operator<<(std::ostream & out, BankException & ex);

#endif

