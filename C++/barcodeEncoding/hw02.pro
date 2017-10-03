TEMPLATE = app
CONFIG += console c++11
CONFIG -= app_bundle
CONFIG -= qt

SOURCES += main.cpp \
    barcode.cpp \
    barcodeean13.cpp \
    barcodedatamatrix.cpp \
    barcodeean13test.cpp

HEADERS += \
    barcode.h \
    barcodeean13.h \
    barcodedatamatrix.h \
    catch.hpp

