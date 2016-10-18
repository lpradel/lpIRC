lpIRC
==================

[![AppVeyor Build status](https://ci.appveyor.com/api/projects/status/github/lpradel/lpIRC?branch=master&svg=true)](https://ci.appveyor.com/project/lpradel/lpirc)

My lightweight (~50 KB) IRC client implementing RFC 1459.

![Demo video](https://cloud.githubusercontent.com/assets/582842/19410084/4346cdcc-92e3-11e6-89ea-6efd18bf3757.gif)

## Installation

Compile the client using Microsoft Visual Studio. So far I have tested it on Windows only using the following compilers:
- MVS 2010
- MVS 2013
- MVS 2015


### Dependencies

The client does not depend on any libraries.

## Usage

Open the client and click on `Connection` -> `Connect...`.

Here, you need to enter:
- the server address (e.g. `irc.quakenet.org`)
- the port number (e.g. `6667`)
- your username
- your real name
- your nicknames (these are separated by spaces)

If you tick the checkbox `Save data`, the client will remember your connection settings the next time you open it.

Finally, click on `Connect` and wait until you are connected to the IRC server. All server interactions will be logged in the main window. Now, just enter the usual IRC commands and send them using the Enter key.

For a simple demonstration refer to the video above.

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :)

## History

- Version 1.0: Initial release.

## Credits

- [Lukas Pradel](https://github.com/lpradel)

## License


    Copyright 2016 Lukas Pradel
    
    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at
    
      http://www.apache.org/licenses/LICENSE-2.0
    
    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.