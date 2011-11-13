#pragma once

namespace citylizard
{
namespace cast
{

class default_
{
public:

    class value
    {
    public:

        template<class T>
        operator T() const 
        { 
            return T(); 
        }
    }; // class value

}; // class default_

} // namespace cast
} // namespace citylizard

