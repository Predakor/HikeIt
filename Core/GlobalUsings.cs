global using Core.Extentions;
global using Core.Results;
global using Core.Validators;

namespace Core;

class Blocker {
    AbstractValidator<object> validator;

    string[] x = [" c", "d "];

    void xd() {
        if (x.NotNullOrEmpty()) { }
    }
}
