#pragma once

#ifndef STRICT
#define STRICT
#endif

#define _ATL_APARTMENT_THREADED
#define _ATL_NO_AUTOMATIC_NAMESPACE
#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS
#define ATL_NO_ASSERT_ON_DESTROY_NONEXISTENT_WINDOW

#include "targetver.h"
#include "resource.h"

#include <atlbase.h>
#include <atlcom.h>
#include <atlfile.h>
#include <atlstr.h>

#include <ShlObj.h>
