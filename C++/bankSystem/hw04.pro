TEMPLATE = app
CONFIG += console c++11
CONFIG -= app_bundle
CONFIG -= qt

SOURCES += main.cpp \
    Account.cpp \
    Bank.cpp \
    BankRegister.cpp \
    Customer.cpp \
    Database.cpp \
    Interactive.cpp \
    showcase_main.cpp

HEADERS += \
    Account.h \
    AccountData.h \
    AccountID.h \
    AccountInterface.h \
    Bank.h \
    BankExceptions.h \
    BankRegister.h \
    Customer.h \
    CustomerData.h \
    CustomerInterface.h \
    Database.h \
    Interactive.h

