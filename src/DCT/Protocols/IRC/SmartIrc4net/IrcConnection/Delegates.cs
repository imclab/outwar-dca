/*
 * $Id: Delegates.cs 201 2005-06-09 17:06:22Z meebey $
 * $URL: svn://svn.qnetp.net/smartirc/SmartIrc4net/tags/0.4.0/src/IrcConnection/Delegates.cs $
 * $Rev: 201 $
 * $Author: meebey $
 * $Date: 2005-06-09 19:06:22 +0200 (Thu, 09 Jun 2005) $
 *
 * SmartIrc4net - the IRC library for .NET/C# <http://smartirc4net.sf.net>
 *
 * Copyright (c) 2003-2005 Mirco Bauer <meebey@meebey.net> <http://www.meebey.net>
 *
 * Full LGPL License: <http://www.gnu.org/licenses/lgpl.txt>
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General internal
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General internal License for more details.
 *
 * You should have received a copy of the GNU Lesser General internal
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

namespace Meebey.SmartIrc4net
{
    internal delegate void ReadLineEventHandler(object sender, ReadLineEventArgs e);
    internal delegate void WriteLineEventHandler(object sender, WriteLineEventArgs e);
    internal delegate void AutoConnectErrorEventHandler(object sender, AutoConnectErrorEventArgs e);
}