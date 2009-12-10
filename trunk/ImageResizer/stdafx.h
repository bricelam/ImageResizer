#pragma once

#ifndef STRICT
#define STRICT
#endif

#include "targetver.h"

#define _ATL_APARTMENT_THREADED
#define _ATL_NO_AUTOMATIC_NAMESPACE
#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS

#include "Resource.h"

// WIN32
#include <ShlObj.h>

// ATL
#include <atlcoll.h>
#include <atlcom.h>
#include <atlpath.h>
#include <atltypes.h>
#include <atlwin.h>

// GDI+
#include <GdiPlus.h>
