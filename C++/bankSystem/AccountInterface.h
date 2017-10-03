#ifndef ACCOUNTINTERFACE_H
#define ACCOUNTINTERFACE_H

#include "AccountID.h"

class AccountInterface
{
public:
    // Aby nedocházelo k leakum pri mazani pripadnych potomku:
    virtual ~AccountInterface() {}
    
    virtual void deposit(uint amount) = 0;
    virtual void withdraw(uint amount) = 0;
    virtual void transfer(uint amount, AccountID target) = 0;

    virtual uint balance() const = 0;
    virtual uint number() const = 0;
};

#endif
