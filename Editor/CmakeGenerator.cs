

namespace cmake
{
    enum CMakeGenerator
    {
        VS16,
        VisualStudio2019 = VS16,
        VS15,
        VisualStudio2017 = VS15,
        VS14,
        VisualStudio2015 = VS14,
        VS12,
        VisualStudio2013 = VS12,
        VS11,
        VisualStudio2012 = VS11,
        VS10,
        VisualStudio2010 = VS10,
        VS9,
        VisualStudio2008 = VS9,

        NMakeMakefiles,
        JomMakefiles,
        UnixMakefiles,
        Ninja,
    }
}
