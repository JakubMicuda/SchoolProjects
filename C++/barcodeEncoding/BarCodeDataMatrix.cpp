/**
 * @Author Jakub Micuda(433715)
 */
#include "BarCodeDataMatrix.h"
#include <iostream>

using namespace std;

CBarCodeDataMatrix::CBarCodeDataMatrix(const string& data, size_t dataMatrixSize) : m_data(nullptr), m_size(dataMatrixSize) {
    parseInput(data);
}

size_t CBarCodeDataMatrix::size() const {
    return m_size;
}

void CBarCodeDataMatrix::zero() {
    if(isValid()) {
        for(unsigned int i=0; i<m_size; i++) {
            for(unsigned int j=0; j<m_size; j++) {
                m_data[i][j] = false;
            }
        }
    }else cerr << "WARNING: You are trying to use zero() on invalid matrix!" << endl;
}

void CBarCodeDataMatrix::print() const{
    if(isValid()) {
        for(unsigned int i=0; i<m_size; i++) {
            for(unsigned int j=0; j<m_size; j++) {
                if(m_data[i][j]) cout << '#';
                else cout << ' ';
            }
            if(i!=m_size-1) cout << endl;       //need this condition beacuse print shouldn't end with endl
        }
    }else cerr << "WARNING: You are trying to use print() on invalid matrix!" << endl;
}

bool CBarCodeDataMatrix::isValid() const {
    if(m_data == nullptr) return false;             //if m_data equals nullptr it means, allocating failed
    for(unsigned int i=0; i<m_size; i++) {
        if(m_data[i] == nullptr) return false;      //also checking every element if it was succesfully allocated
    }
    return true;
}

bool CBarCodeDataMatrix::parseInput(const string& data) {
    //setting m_data to nullptr if given size was 0, otherwise it allocates memory by given size
    if(static_cast<signed>(m_size)<=0) m_data = nullptr;
    else {
        m_data = new(nothrow) bool*[m_size];
        if(m_data != nullptr) {
            for(unsigned int i=0; i<m_size; i++) {
                m_data[i] = new(nothrow) bool[m_size];
            }
        }
    }

    //checks if allocation was succesful, if it wasn't, returns false
    if(!isValid()) return false;
    if(&data == nullptr) return false;

    //clearing matrix, and filling it by false
    zero();
    unsigned int i,j,k;
    i = j = k = 0;

    //loops while matrix isn't filled or while string is not on last character
    while(i<m_size && &data[k] != &data.back()) {
        j=0;
        while(j<m_size && &data[k] != &data.back()) {
            if(data[k] != '0' && data[k] != '1' && !isspace(data[k])) return false; //checks validity of current character
            if(data[k] == '0') {
                m_data[i][j] = false;
                j++;
            }
            if(data[k] == '1') {
                m_data[i][j] = true;
                j++;
            }
            k++;
        }
        i++;
    }

    //if current character is last, and matrix isn't filled it still needs to be checked
    if(i<=m_size && j<m_size) {
        if(data[k] != '0' && data[k] != '1' && !isspace(data[k])) return false;
        if(data[k] == '0') {
            m_data[i-1][j] = false;
            j++;
        }
        if(data[k] == '1') {
            m_data[i-1][j] = true;
            j++;
        }
    }

    //if matrix still isn't filled, method returns false
    if(!(i==m_size && j==m_size)) return false;
    return true;
}

void CBarCodeDataMatrix::exportToSVG(size_t scale, size_t offsetX, size_t offsetY, bool svgHeader) const {
    if(svgHeader) {
       //printing header of svg file
        cout << XMLVersion << endl;
        cout << Doctype << endl;
        cout << SVGHeader << endl;
    }

    string endElement = "</svg>";

    //if data are invalid method prints empty svg file
    if(!isValid()) {
        if(svgHeader)
            cout << endElement << endl;
        return;
    }

    size_t pom;

    //printing each element
    for(unsigned int i=0; i<m_size; i++) {
        pom = offsetX;
        for(unsigned int j=0; j<m_size; j++) {
            recMake(pom,offsetY,scale,m_data[i][j]);
            pom += scale;
        }
        offsetY += scale;
    }

    //printing ending svg tag
    if(svgHeader)
        cout << endElement << endl;
}

void CBarCodeDataMatrix::recMake(size_t offsetX, size_t offsetY, size_t scale, bool data) const {

    string color;
    if(data) color = "black";
    else color = "white";

    cout << "\t<rect x=\"" << offsetX << "\" y=\"";
    cout << offsetY << "\" width=\"" << scale;
    cout << "\" height=\"" << scale << "\" fill=\"";
    cout << color << "\" />" << endl;
}

CBarCodeDataMatrix::~CBarCodeDataMatrix() {
    if(isValid()) {
        for(unsigned int i=0; i<m_size; i++) {
            delete [] m_data[i];
        }
        delete [] m_data;
        m_data = nullptr;
    }
}
