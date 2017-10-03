/**
 *@author Jakub Micuda(433715)
 */
#ifndef ACCOUNT_H
#define ACCOUNT_H

#include "AccountInterface.h"
#include <algorithm>

typedef unsigned int uint;

//this header doesn't need full implementation of these classes
class Bank;
class Database;

/**
 * @brief Interface for manipulating accounts data of bank
 */
class BasicAccount : public AccountInterface
{
private:
    uint accountID;
    Database* myDatabase;
    Bank* myBank;
public:
    /**
     * @brief Parametric Constructor of class BasicAccount
     * @param number ID of account in database
     * @param bank  pointer to customer's bank
     * @param db pointer to bank's database
     */
    BasicAccount(uint number, Bank* bank, Database* db);

    /**
     * @brief Deposits money to account
     * @param amount  amount of money to be deposited
     */
    void deposit(uint amount);

    /**
     * @brief Withdraws money from account, possible exception if there isnt enough money
     * @param amount   amount of money to be withdrawn
     */
    void withdraw(uint amount);

    /**
     * @brief Method transfers amount of money from this account to target account
     * @param amount    amount of money to be transfered
     * @param target    target account
     */
    void transfer(uint amount, AccountID target);

    /**
     * @brief Getter for balance on account
     * @return balance from database
     */
    uint balance() const;

    /**
     * @brief Getter for ID of account
     * @return ID of account as number
     */
    uint number() const;

    /**
     *  Destructor of BasicAccount
     */
    ~BasicAccount();
};

#endif
