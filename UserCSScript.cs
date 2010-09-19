using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Windows.Forms;

namespace Ketarin
{
    public interface ICustomSetupScript
    {
        void Execute(IWin32Window owner, ApplicationJob app);
    }

    /// <summary>
    /// Represents a C# script that can be compiled and executed.
    /// </summary>
    public class UserCSScript
    {
        /// <summary>
        /// Custom scripts should not require namespace declarations or classes.
        /// </summary>
        private const string codeTemplate = @"
        using System;
        using System.Collections.Generic;
        using System.Text;
        using System.IO;
        using System.Windows.Forms;

        namespace Ketarin
        {
            public class CustomScript : Ketarin.ICustomSetupScript
            {
                public void Execute(IWin32Window owner, ApplicationJob app)
                {
                {0}
                }
                
                public void Abort(string error)
                {
                    throw new ApplicationException(error);
                }
            }
        }
";

        #region Properties

        /// <summary>
        /// Gets or sets the CS-Code to execute.
        /// </summary>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// In order to understand compiler errors, this determines the line at which the user code starts.
        /// </summary>
        internal int LineAtCodeStart
        {
            get
            {
                string[] lines = codeTemplate.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("{0}")) return i + 1;
                }
                return 0;
            }
        }

        #endregion

        /// <summary>
        /// Creates a new instance of a C# script with given code.
        /// </summary>
        public UserCSScript(string code)
        {
            this.Code = code;
        }

        /// <summary>
        /// Compiles the user code into an assembly.
        /// </summary>
        /// <param name="errors">Compiler errors if any</param>
        public Assembly Compile(out CompilerErrorCollection errors)
        {
            // Create a code provider
            // This class implements the 'CodeDomProvider' class as its base. All of the current .Net languages (at least Microsoft ones)
            // come with thier own implemtation, thus you can allow the user to use the language of thier choice (though i recommend that
            // you don't allow the use of c++, which is too volatile for scripting use - memory leaks anyone?)
            Microsoft.CSharp.CSharpCodeProvider csProvider = new Microsoft.CSharp.CSharpCodeProvider();

            // Setup our options
            CompilerParameters options = new CompilerParameters();
            options.GenerateExecutable = false; // we want a Dll (or "Class Library" as its called in .Net)
            options.GenerateInMemory = true; // Saves us from deleting the Dll when we are done with it, though you could set this to false and save start-up time by next time by not having to re-compile
            // And set any others you want, there a quite a few, take some time to look through them all and decide which fit your application best!

            // Add any references you want the users to be able to access, be warned that giving them access to some classes can allow
            // harmful code to be written and executed. I recommend that you write your own Class library that is the only reference it allows
            // thus they can only do the things you want them to.
            // (though things like "System.Xml.dll" can be useful, just need to provide a way users can read a file to pass in to it)
            // Just to avoid bloatin this example to much, we will just add THIS program to its references, that way we don't need another
            // project to store the interfaces that both this class and the other uses. Just remember, this will expose ALL public classes to
            // the "script"
            options.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            options.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            options.ReferencedAssemblies.Add("System.dll");

            // Compile our code
            CompilerResults result;
            result = csProvider.CompileAssemblyFromSource(options, codeTemplate.Replace("{0}", this.Code));

            errors = result.Errors;
            return errors.HasErrors ? null : result.CompiledAssembly;
        }

        public void Execute(ApplicationJob argument)
        {
            CompilerErrorCollection errors;
            Assembly assembly = Compile(out errors);

            if (errors.HasErrors)
            {
                throw new ApplicationException("Script cannot be compiled: " + errors[0].ErrorText);
            }

            // Now that we have a compiled script, lets run them
            foreach (Type type in assembly.GetExportedTypes())
            {
                foreach (Type iface in type.GetInterfaces())
                {
                    if (iface != typeof(ICustomSetupScript)) continue;

                    // yay, we found a script interface, lets create it and run it!

                    // Get the constructor for the current type
                    // you can also specify what creation parameter types you want to pass to it,
                    // so you could possibly pass in data it might need, or a class that it can use to query the host application
                    ConstructorInfo constructor = type.GetConstructor(System.Type.EmptyTypes);
                    if (constructor != null && constructor.IsPublic)
                    {
                        // lets be friendly and only do things legitimitely by only using valid constructors

                        // we specified that we wanted a constructor that doesn't take parameters, so don't pass parameters
                        ICustomSetupScript scriptObject = constructor.Invoke(null) as ICustomSetupScript;
                        if (scriptObject != null)
                        {
                            scriptObject.Execute(null, argument);
                        }
                        else
                        {
                            // hmmm, for some reason it didn't create the object
                            // this shouldn't happen, as we have been doing checks all along, but we should
                            // inform the user something bad has happened, and possibly request them to send
                            // you the script so you can debug this problem
                            // floele: Should not occur without an exception anyway.
                        }
                    }
                    else
                    {
                        // and even more friendly and explain that there was no valid constructor
                        // found and thats why this script object wasn't run
                        // floele: Our scripts will automatically have a valid constructor.
                    }
                }
            }
        }
    }
}
