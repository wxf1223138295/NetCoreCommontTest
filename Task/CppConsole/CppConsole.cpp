#include "pch.h"
#using <TestCpp.dll>
using namespace TestCpp;
using namespace System;

int main(array<System::String ^> ^args)
{
    int sum;
    TestCpp::ShawnShow^ math = gcnew TestCpp::ShawnShow();
    sum=math->Add(1, 2);

    return sum;
}
