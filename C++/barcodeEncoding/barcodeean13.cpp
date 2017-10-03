/**
 * @Author Jakub Micuda(433715)
 */
#include "BarCodeEAN13.h"
#include <cctype>
#include <cstdlib>
#include <iostream>

using namespace std;

const string CBarCodeEAN13::ParityEncodings[] = {"111111", "110100", "110010", "110001", "101100", "100110", "100011", "101010", "101001", "100101"};
const string CBarCodeEAN13::LeftOddCoding[] = {"0001101", "0011001", "0010011", "0111101", "0100011", "0110001", "0101111", "0111011", "0110111", "0001011"};
const string CBarCodeEAN13::LeftEvenCoding[] = {"0100111", "0110011", "0011011", "0100001", "0011101", "0111001", "0000101", "0010001", "0001001", "0010111"};
const string CBarCodeEAN13::RightCoding[] = {"1110010", "1100110", "1101100", "1000010", "1011100", "1001110", "1010000", "1000100", "1001000", "1110100"};

CBarCodeEAN13::CBarCodeEAN13(const int *eanCode) {
    if(!eanCode) zero();        //if there is nothing given, method zero() is called.
    else {
    for(int i=0; i<12; i++) {
        m_digits[i] = eanCode[i];
    }
    m_checksum = computeCheckSum();
    encode();
    }
}

CBarCodeEAN13::CBarCodeEAN13(const string& eanCode) {
    parseInput(eanCode);
}

void CBarCodeEAN13::print() const {
    for(unsigned int i=0; i<12; i++) {
        cout << m_digits[i];
    }
    cout << m_checksum;
}

void CBarCodeEAN13::zero() {
    for(unsigned int i=0; i<12; i++) {
        m_digits[i] = 0;
    }
    m_checksum = computeCheckSum();
    encode();
}

bool CBarCodeEAN13::isValid() const {
    for(int i=0; i<12; i++) {
        if(!(m_digits[i] >=0 && m_digits[i] <10)) return false;
    }
    if(m_checksum != computeCheckSum()) return false;
    return true;
}

bool CBarCodeEAN13::parseInput(const string& data) {
    zero();                             //clearing m_digits array at start(setting everything to 0)
    if(data.empty()) return false;      //if there is nothing given, method returns false

    unsigned int i=0;
    unsigned int j=0;

    while(j<12 && &data[i] != &data.back()) {           //loops while array isn't filled or while string isn't on the last character
        if(!isdigit(data[i]) && !isspace(data[i])) {    //checks validity of current character
            m_checksum = computeCheckSum();
            return false;
        }

        if(isdigit(data[i])) {
            m_digits[j] = data[i] - '0';
            j++;
        }
        i++;
    }

    //if loop didn't end because of filling whole array, last character still needs to be checked
    if(j<12) {
        if(!isdigit(data[i]) && !isspace(data[i])) {
            m_checksum = computeCheckSum();
            return false;
        }
        if(isdigit(data[i])) {
            m_digits[j] = data[i] - '0';
            j++;
        }
    }
    m_checksum = computeCheckSum();

    //if m_digits still isn't filled, method has to return false
    if(j!=12) return false;
    encode();
    return true;
}

int* CBarCodeEAN13::digits() {
    return m_digits;
}

int CBarCodeEAN13::checkSum() const {
    return m_checksum;
}

int CBarCodeEAN13::computeCheckSum() const {
    int check = 0;
    for(unsigned int i=0; i<12; i++)
        {
            if(i % 2) check += 3 * m_digits[i];     //computes checksum by parity of each element
            else check += m_digits[i];
        }
    return (10 - (check % 10)) % 10;
}

void CBarCodeEAN13::encode() {
    if(!isValid()) return;                  //if data are invalid, method ends

    string parity = ParityEncodings[m_digits[0]]; //first number determines how to encode next 6 numbers

    //encoding 2. - 7. number
    for(unsigned int i=0;i<6; i++) {
        if(parity[i]=='1') coded[i] = LeftOddCoding[m_digits[i+1]];
        else coded[i] = LeftEvenCoding[m_digits[i+1]];
    }
    //encoding 8. - 12. number
    for(unsigned int i=6; i<11; i++) {
        coded[i] = RightCoding[m_digits[i+1]];
    }

    //encoding checksum
    coded[11] = RightCoding[m_checksum];
}

void CBarCodeEAN13::exportToSVG(size_t scale, size_t offsetX, size_t offsetY, bool svgHeader) const {
    if(svgHeader) {
        //putting headers on standard output
        cout << XMLVersion << endl;
        cout << Doctype << endl;
        cout << SVGHeader << endl;
    }

   string endElement = "</svg>";

   //if data are invalid, method prints empty svg file
   if(!isValid()) {
       if(svgHeader)
            cout << endElement << endl;
       return;
   }

   //setting svg parameters by template
   size_t width = scale * 95;
   size_t segHeight = width * 0.65;
   size_t numHeight = segHeight * 0.90;
   size_t currOffsetX = offsetX;

   //printing starting segment
   recMake(currOffsetX,offsetY,scale,segHeight,"black");
   currOffsetX += scale;
   recMake(currOffsetX,offsetY,scale,segHeight,"white");
   currOffsetX += scale;
   recMake(currOffsetX,offsetY,scale,segHeight,"black");
   currOffsetX += scale;

   //printing first 6 encoded binary numbers
   for(unsigned int i=0; i<6; i++) {
       for(unsigned int j=0; j<7; j++) {
           if(coded[i][j] == '1') recMake(currOffsetX,offsetY,scale,numHeight,"black");
           else recMake(currOffsetX,offsetY,scale,numHeight,"white");
           currOffsetX += scale;
       }
   }

   //printing middle segment
   recMake(currOffsetX,offsetY,scale,segHeight,"white");
   currOffsetX += scale;
   recMake(currOffsetX,offsetY,scale,segHeight,"black");
   currOffsetX += scale;
   recMake(currOffsetX,offsetY,scale,segHeight,"white");
   currOffsetX += scale;
   recMake(currOffsetX,offsetY,scale,segHeight,"black");
   currOffsetX += scale;
   recMake(currOffsetX,offsetY,scale,segHeight,"white");
   currOffsetX += scale;

   //printing next 6 encoded binary numbers
   for(unsigned int i=6; i<12; i++) {
       for(unsigned int j=0; j<7; j++) {
           if(coded[i][j] == '1') recMake(currOffsetX,offsetY,scale,numHeight,"black");
           else recMake(currOffsetX,offsetY,scale,numHeight,"white");
           currOffsetX += scale;
       }
   }

   //printing end segment
   recMake(currOffsetX,offsetY,scale,segHeight,"black");
   currOffsetX += scale;
   recMake(currOffsetX,offsetY,scale,segHeight,"white");
   currOffsetX += scale;
   recMake(currOffsetX,offsetY,scale,segHeight,"black");
   currOffsetX += scale;

   //setting svg text parametes by template
   size_t currOffsetY;
   currOffsetY = offsetY + ((scale * 10) * 6.35);
   currOffsetX = offsetX - (8 * scale);

   //printing first digit
   textMake(currOffsetX, currOffsetY, scale, m_digits[0]);

   currOffsetX = offsetX + (5 * scale);

   //printing next 6 digits
   for(unsigned int i=1; i<7; i++) {
       textMake(currOffsetX,currOffsetY, scale, m_digits[i]);
       if(i!=6)currOffsetX += (7 * scale);
   }

   currOffsetX += (scale * 10);

   //printing next 5 digits
   for(unsigned int i=7; i<12; i++) {
       textMake(currOffsetX,currOffsetY, scale, m_digits[i]);
       currOffsetX += (7 * scale);
   }

   //printing m_checksum as last digit
   textMake(currOffsetX, currOffsetY, scale, m_checksum);

   //printing ending svg tag
   if(svgHeader)
     cout << endElement << endl;
}

void CBarCodeEAN13::recMake(size_t offsetX, size_t offsetY, size_t width, size_t height, string color) const {
  cout << "\t<rect x=\"" << offsetX << "\" y=\"";
  cout << offsetY << "\" width=\"" << width;
  cout << "\" height=\"" << height << "\" fill=\"";
  cout << color << "\" />" << endl;
}

void CBarCodeEAN13::textMake(size_t offsetX, size_t offsetY, size_t scale, int number) const {
    cout << "\t<text x=\"" << offsetX << "\" y=\"";
    cout << offsetY << "\" style=\"font-size:" << scale * 10;
    cout << "px\">" << number << "</text>" << endl;
}
