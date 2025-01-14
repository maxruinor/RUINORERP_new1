﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;

namespace RUINORERP.UI.AdvancedQuery
{
    public class addpro
    {
        public static void Addp()
        {
            // specify a new assembly name
            var assemblyName = new AssemblyName("Pets");

            // create assembly builder
            var assemblyBuilder = AppDomain.CurrentDomain
              .DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            // create module builder
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("PetsModule", "Pets.dll");

            // create type builder for a class
            var typeBuilder = moduleBuilder.DefineType("Kitty", TypeAttributes.Public);

            // then create whole class structure
            CreateKittyClassStructure(typeBuilder);

            // then create the whole class type
            var classType = typeBuilder.CreateType();

            // save assembly
            assemblyBuilder.Save("Pets.dll");

            Console.WriteLine("Hi, Dennis, a Pets assembly has been generated for you.");
            Console.ReadLine();
        }

        private static void CreateKittyClassStructure(TypeBuilder typeBuilder)
        {
            // ---- define fields ----

            var fieldId = typeBuilder.DefineField(
              "_id", typeof(int), FieldAttributes.Private);
            var fieldName = typeBuilder.DefineField(
              "_name", typeof(string), FieldAttributes.Private);

            // ---- define costructors ----

            Type objType = Type.GetType("System.Object");
            ConstructorInfo objCtor = objType.GetConstructor(new Type[0]);

            Type[] constructorArgs = { typeof(int), typeof(string) };

            var constructorBuilder = typeBuilder.DefineConstructor(
               MethodAttributes.Public, CallingConventions.Standard, constructorArgs);
            ILGenerator ilOfCtor = constructorBuilder.GetILGenerator();

            ilOfCtor.Emit(OpCodes.Ldarg_0);
            ilOfCtor.Emit(OpCodes.Call, objCtor);
            ilOfCtor.Emit(OpCodes.Ldarg_0);
            ilOfCtor.Emit(OpCodes.Ldarg_1);
            ilOfCtor.Emit(OpCodes.Stfld, fieldId);
            ilOfCtor.Emit(OpCodes.Ldarg_0);
            ilOfCtor.Emit(OpCodes.Ldarg_2);
            ilOfCtor.Emit(OpCodes.Stfld, fieldName);
            ilOfCtor.Emit(OpCodes.Ret);

            // ---- define properties ----

            var methodGetId = typeBuilder.DefineMethod(
              "GetId", MethodAttributes.Public, typeof(int), null);
            var methodSetId = typeBuilder.DefineMethod(
              "SetId", MethodAttributes.Public, null, new Type[] { typeof(int) });

            var ilOfGetId = methodGetId.GetILGenerator();
            ilOfGetId.Emit(OpCodes.Ldarg_0); // this
            ilOfGetId.Emit(OpCodes.Ldfld, fieldId);
            ilOfGetId.Emit(OpCodes.Ret);

            var ilOfSetId = methodSetId.GetILGenerator();
            ilOfSetId.Emit(OpCodes.Ldarg_0); // this
            ilOfSetId.Emit(OpCodes.Ldarg_1); // the first one in arguments list
            ilOfSetId.Emit(OpCodes.Stfld, fieldId);
            ilOfSetId.Emit(OpCodes.Ret);

            // create Id property
            var propertyId = typeBuilder.DefineProperty(
              "Id", PropertyAttributes.None, typeof(int), null);
            propertyId.SetGetMethod(methodGetId);
            propertyId.SetSetMethod(methodSetId);

            var methodGetName = typeBuilder.DefineMethod(
              "GetName", MethodAttributes.Public, typeof(string), null);
            var methodSetName = typeBuilder.DefineMethod(
              "SetName", MethodAttributes.Public, null, new Type[] { typeof(string) });

            var ilOfGetName = methodGetName.GetILGenerator();
            ilOfGetName.Emit(OpCodes.Ldarg_0); // this
            ilOfGetName.Emit(OpCodes.Ldfld, fieldName);
            ilOfGetName.Emit(OpCodes.Ret);

            var ilOfSetName = methodSetName.GetILGenerator();
            ilOfSetName.Emit(OpCodes.Ldarg_0); // this
            ilOfSetName.Emit(OpCodes.Ldarg_1); // the first one in arguments list
            ilOfSetName.Emit(OpCodes.Stfld, fieldName);
            ilOfSetName.Emit(OpCodes.Ret);

            // create Name property
            var propertyName = typeBuilder.DefineProperty(
              "Name", PropertyAttributes.None, typeof(string), null);
            propertyName.SetGetMethod(methodGetName);
            propertyName.SetSetMethod(methodSetName);

            // ---- define methods ----

            // create ToString() method
            var methodToString = typeBuilder.DefineMethod(
              "ToString",
              MethodAttributes.Virtual | MethodAttributes.Public,
              typeof(string),
              null);

            var ilOfToString = methodToString.GetILGenerator();
            var local = ilOfToString.DeclareLocal(typeof(string)); // create a local variable
            ilOfToString.Emit(OpCodes.Ldstr, "Id:[{0}], Name:[{1}]");
            ilOfToString.Emit(OpCodes.Ldarg_0); // this
            ilOfToString.Emit(OpCodes.Ldfld, fieldId);
            ilOfToString.Emit(OpCodes.Box, typeof(int)); // boxing the value type to object
            ilOfToString.Emit(OpCodes.Ldarg_0); // this
            ilOfToString.Emit(OpCodes.Ldfld, fieldName);
            ilOfToString.Emit(OpCodes.Call,
              typeof(string).GetMethod("Format",
              new Type[] { typeof(string), typeof(object), typeof(object) }));
            ilOfToString.Emit(OpCodes.Stloc, local); // set local variable
            ilOfToString.Emit(OpCodes.Ldloc, local); // load local variable to stack
            ilOfToString.Emit(OpCodes.Ret);
        }


    }
}

