#region License

// Distributed under the BSD License
//   
// YouTrackSharp Copyright (c) 2010-2012, Hadi Hariri and Contributors
// All rights reserved.
//   
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//      * Redistributions of source code must retain the above copyright
//         notice, this list of conditions and the following disclaimer.
//      * Redistributions in binary form must reproduce the above copyright
//         notice, this list of conditions and the following disclaimer in the
//         documentation and/or other materials provided with the distribution.
//      * Neither the name of Hadi Hariri nor the
//         names of its contributors may be used to endorse or promote products
//         derived from this software without specific prior written permission.
//   
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
//   TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
//   PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
//   <COPYRIGHTHOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//   SPECIAL,EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//   LIMITED  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//   DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  ON ANY
//   THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
//   THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//   

#endregion

using System;
using System.Linq;
using System.Reflection;
using CommandLine;

namespace YouTrackSharp.Commands
{
    public class CommandParser
    {
        public Command Parse(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                throw new CommandException("Emtpy command");
            }

            var commandAndParams = (from words in inputString
                                    let split = inputString.Split(' ')
                                    select new {Name = split.First(), Parameters = split.Skip(1)}).FirstOrDefault();

            if (commandAndParams == null)
            {
                throw new CommandException("Invalid Command");
            }

            var commandOptionsType =
                Assembly.GetExecutingAssembly().GetType(string.Format("{0}.{1}.{2}CommandOptions", Assembly.GetExecutingAssembly().GetName().Name, "Commands.CommandOptions", commandAndParams.Name));

            if (commandOptionsType == null)
            {
                throw new CommandException("Invalid Command");
            }

            var commandParameters = Activator.CreateInstance(commandOptionsType);

            ICommandLineParser parser = new CommandLineParser(new CommandLineParserSettings(false));

            if (parser.ParseArguments(commandAndParams.Parameters.ToArray(), commandParameters))
            {
                return new Command() {Name = commandAndParams.Name, Parameters = commandParameters};
            }
            throw new CommandException("Invalid Command Parameters");
        }
    }
}