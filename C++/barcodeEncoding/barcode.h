#ifndef PB161_HW02_BARCODE_H
#define PB161_HW02_BARCODE_H

#include <ostream>
#include <string>

class CBarCode {
public:
    static const std::string XMLVersion;
    static const std::string Doctype;
    static const std::string SVGHeader;

public:
    /**
     * Constructor of the base class, can be called only from the constructors of inherited classes.
    */
    CBarCode() {}

    /**
     * Destructor of the base class, has to be virtual if any of the descendants allocates any resources which need to be taken care of beforehand.
    */
    virtual ~CBarCode() {}

    /**
     * Zeroes the content of the barcode.
    */
    virtual void zero() = 0;

    /**
     * Outputs the content of the barcode on a console window.
    */
    virtual void print() const = 0;

    /**
     * Depends on the descendant class.
     *
     * @return	TRUE if the content of the barcode is valid, FALSE otherwise.
    */
    virtual bool isValid() const = 0;

    /**
     * Parses the input string and fills the barcode with its content.
     *
     * @param[in]	data	String to be parsed.
     * @return		TRUE if the string was successfully parsed(i.e. contained enough data and no unknown characters), FALSE otherwise.
    */
    virtual bool parseInput(const std::string& data) = 0;

    /**
     * Exports the content of the barcode to the SVG file. (In our case standard output will do.)
     *
     * @param[in]	scale		Determines the scale of the barcode, depending on the context.
     * @param[in]	offsetX		Sets the distance from the point [0,0] to the left side of the barcode (offset on the X-axis).
     * @param[in]	offsetY		Sets the distance from the point [0,0] to the top side of the barcode (offset on the Y-axis).
     * @param[in]	svgHeader	Exports the barcode data alongside with the SVG header and closing element (optional argument).
     * @return		TRUE if the barcode was exported successfully, FALSE if anything went wrong.
    */
    virtual void exportToSVG(std::size_t scale, std::size_t offsetX, std::size_t offsetY, bool svgHeader = false) const = 0;
};

#endif
