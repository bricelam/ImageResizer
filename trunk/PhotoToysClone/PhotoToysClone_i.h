

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0550 */
/* at Mon Feb 16 18:12:51 2009
 */
/* Compiler settings for .\PhotoToysClone.idl:
    Oicf, W1, Zp8, env=Win64 (32b run), target_arch=AMD64 7.00.0550 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__


#ifndef __PhotoToysClone_i_h__
#define __PhotoToysClone_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __PhotoToysShellExtension_FWD_DEFINED__
#define __PhotoToysShellExtension_FWD_DEFINED__

#ifdef __cplusplus
typedef class PhotoToysShellExtension PhotoToysShellExtension;
#else
typedef struct PhotoToysShellExtension PhotoToysShellExtension;
#endif /* __cplusplus */

#endif 	/* __PhotoToysShellExtension_FWD_DEFINED__ */


#ifdef __cplusplus
extern "C"{
#endif 



#ifndef __PhotoToysCloneLib_LIBRARY_DEFINED__
#define __PhotoToysCloneLib_LIBRARY_DEFINED__

/* library PhotoToysCloneLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_PhotoToysCloneLib;

EXTERN_C const CLSID CLSID_PhotoToysShellExtension;

#ifdef __cplusplus

class DECLSPEC_UUID("4CF20B46-D006-4B90-A64B-DBAA9470EFBE")
PhotoToysShellExtension;
#endif
#endif /* __PhotoToysCloneLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


