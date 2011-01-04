#pragma once

namespace citylizard
{
namespace intrusive
{
/// User-defined types.
namespace user
{

/// User-defined traits.
template<class T, class Enable = void> 
class traits
{
public:
    /// Add reference.
    /// &r != 0.
    static void add(T &r);
    /// Release reference.
    /// &r != 0.
    static void release(T &r);
    /// Bad reference exception.
    class bad_ref_t
    {
    };
    /// Bad reference generator.
    typedef bad_ref_t bad_ref;
};

}
}
}
