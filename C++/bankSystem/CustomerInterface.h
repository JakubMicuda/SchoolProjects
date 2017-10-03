/**
 *@author Jakub Micuda(433715)
 */
#ifndef CUSTOMERINTERFACE_H
#define CUSTOMERINTERFACE_H

#include <vector>
#include <string>

typedef unsigned int uint;

class AccountInterface;

class CustomerInterface
{
public:
    // Aby nedocházelo k leakum pri mazani pripadnych potomku:
    virtual ~CustomerInterface() {}

    virtual AccountInterface* createAccount() = 0;

    virtual std::vector<AccountInterface*> const& accounts() = 0;
    virtual std::string const& name() const = 0;
    virtual uint number() const = 0;
};

#endif
