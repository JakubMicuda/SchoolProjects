/**
 *@author Jakub Micuda(433715)
 */
#ifndef ACCOUNTID_H
#define ACCOUNTID_H

#include <utility>

typedef unsigned int uint;

/**
 * @brief Datatype for Accounts ID
 */
class AccountID
{
private:
    std::pair<uint,uint> accountID;
public:

    /**
     * @brief Parametric Constructor
     * @param accountNumber ID of account in bank
     * @param bankNumber    bank number
     */
    AccountID(uint accountNumber, uint bankNumber);

    /**
     * @brief Nonparametric Constructor
     */
    AccountID();

    /**
     * @brief Getter for accountNumber
     * @return accountNumber
     */
    uint accountNumber() const;

    /**
     * @brief Getter for bankNumber
     * @return bankNumber
     */
    uint bankNumber() const;

/***********************OVERLOADED OPERATORS*****************************/

    AccountID & operator=(const AccountID &);

    friend bool operator==(const AccountID &lhs, const AccountID &rhs) {
        if(lhs.accountID.first != rhs.accountID.first) return false;
        if(lhs.accountID.second != rhs.accountID.second) return false;
        return true;
    }

    friend bool operator!=(const AccountID &lhs, const AccountID &rhs) {
        return !(lhs==rhs);
    }
};

#endif
