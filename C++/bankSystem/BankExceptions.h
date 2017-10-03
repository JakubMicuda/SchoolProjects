#ifndef BANKEXCEPTIONS_H
#define BANKEXCEPTIONS_H

#include <stdexcept>

class BankException : public std::runtime_error
{
public:
    explicit BankException(std::string const& whatMsg) :
        std::runtime_error(whatMsg)
    {}
};

class NonexistenceException : public BankException
{
public:
    explicit NonexistenceException(std::string const& whatMsg) :
        BankException(whatMsg)
    {}
};

class NonexistentCustomer : public NonexistenceException
{
public:
    explicit NonexistentCustomer() :
        NonexistenceException("Such customer does not exist")
    {}
};

class NonexistentAccount : public NonexistenceException
{
public:
    explicit NonexistentAccount() :
        NonexistenceException("Such account does not exist")
    {}
};

class NonexistentBank : public NonexistenceException
{
public:
    explicit NonexistentBank() :
        NonexistenceException("Such bank does not exist")
    {}
};

class TransactionException : public BankException
{
public:
    explicit TransactionException(std::string const& whatMsg) :
        BankException(whatMsg)
    {}
};

class InsufficientFunds : public TransactionException
{
public:
    InsufficientFunds() :
        TransactionException("Insufficient funds to complete the transaction")
    {}
};

#endif