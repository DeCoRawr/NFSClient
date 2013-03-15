/*
 * $Header: /cvsroot/remotetea/remotetea/src/org/acplt/oncrpc/XdrVoid.java,v 1.1.1.1 2003/08/13 12:03:41 haraldalbrecht Exp $
 *
 * Copyright (c) 1999, 2000
 * Lehrstuhl fuer Prozessleittechnik (PLT), RWTH Aachen
 * D-52064 Aachen, Germany.
 * All rights reserved.
 *
 * This library is free software; you can redistribute it and/or modify
 * it under the terms of the GNU Library General Public License as
 * published by the Free Software Foundation; either version 2 of the
 * License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public
 * License along with this program (see the file COPYING.LIB for more
 * details); if not, write to the Free Software Foundation, Inc.,
 * 675 Mass Ave, Cambridge, MA 02139, USA.
 */

using System.Threading;
using System.Net;
using System;
using org.acplt.oncrpc.server;
using System.IO;
namespace org.acplt.oncrpc.apps.jportmap
{
	/// <summary>
	/// The class <code>OncRpcEmbeddedPortmap</code> provides an embeddable
	/// portmap service, which is automatically started in its own thread if
	/// the (operating) system does not already provide the portmap service.
	/// </summary>
	/// <remarks>
	/// The class <code>OncRpcEmbeddedPortmap</code> provides an embeddable
	/// portmap service, which is automatically started in its own thread if
	/// the (operating) system does not already provide the portmap service.
	/// If an embedded portmap service is started it will stop only after the
	/// last ONC/RPC program has been deregistered.
        /// Converted to C# using the db4o Sharpen tool.
	/// </remarks>
	/// <version>$Revision: 1.2 $ $Date: 2003/08/14 08:00:08 $ $State: Exp $ $Locker:  $</version>
	/// <author>Harald Albrecht</author>
        /// <author>Jay Walters</author>
	public class OncRpcEmbeddedPortmap
	{
		/// <summary>
		/// Constructs an embeddable portmap service of class
		/// <code>OncRpcEmbeddedPortmap</code> and starts the service if no
		/// other (external) portmap service is available.
		/// </summary>
		/// <remarks>
		/// Constructs an embeddable portmap service of class
		/// <code>OncRpcEmbeddedPortmap</code> and starts the service if no
		/// other (external) portmap service is available. This constructor is
		/// the same as <code>OncRpcEmbeddedPortmap</code> calling with a
		/// timeout of 3 seconds.
		/// <p>The constructor starts the portmap service in its own thread and
		/// then returns.
		/// </remarks>
		/// <seealso cref="embeddedPortmapInUse()">embeddedPortmapInUse()</seealso>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public OncRpcEmbeddedPortmap() : this(3000)
		{
		}

		/// <summary>
		/// Constructs an embeddable portmap service of class
		/// <code>OncRpcEmbeddedPortmap</code> and starts the service if no
		/// other (external) portmap service is available.
		/// </summary>
		/// <remarks>
		/// Constructs an embeddable portmap service of class
		/// <code>OncRpcEmbeddedPortmap</code> and starts the service if no
		/// other (external) portmap service is available.
		/// <p>The constructor starts the portmap service in its own thread and
		/// then returns.
		/// </remarks>
		/// <param name="checkTimeout">
		/// timeout in milliseconds to wait before assuming
		/// that no portmap service is currently available.
		/// </param>
		/// <seealso cref="embeddedPortmapInUse()">embeddedPortmapInUse()</seealso>
		/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public OncRpcEmbeddedPortmap(int checkTimeout)
		{
			if (!isPortmapRunning(checkTimeout))
			{
				embeddedPortmap = new OncRpcEmbeddedPortmap.embeddedjportmap
					(this);
				embeddedPortmapThread = new OncRpcEmbeddedPortmap.OncRpcEmbeddedPortmapThread
					(this, embeddedPortmap);
                embeddedPortmap.serviceThread = new Thread(new ThreadStart(embeddedPortmapThread.run));
                embeddedPortmap.serviceThread.Name = "Embedded Portmap Service Thread";
                embeddedPortmap.serviceThread.Start();
			}
		}

		/// <summary>
		/// Indicates whether a portmap service (regardless whether it's supplied
		/// by the operating system or an embedded portmap service) is currently
		/// running.
		/// </summary>
		/// <remarks>
		/// Indicates whether a portmap service (regardless whether it's supplied
		/// by the operating system or an embedded portmap service) is currently
		/// running. This method will check for 3 seconds for an answer from a
		/// portmap before assuming that no one exists.
		/// </remarks>
		/// <returns>
		/// <code>true</code>, if a portmap service (either external or
		/// embedded) is running and can be contacted.
		/// </returns>
		public static bool isPortmapRunning()
		{
			return isPortmapRunning(3000);
		}

		/// <summary>
		/// Indicates whether a portmap service (regardless whether it's supplied
		/// by the operating system or an embedded portmap service) is currently
		/// running.
		/// </summary>
		/// <remarks>
		/// Indicates whether a portmap service (regardless whether it's supplied
		/// by the operating system or an embedded portmap service) is currently
		/// running.
		/// </remarks>
		/// <param name="checkTimeout">
		/// timeout in milliseconds to wait before assuming
		/// that no portmap service is currently available.
		/// </param>
		/// <returns>
		/// <code>true</code>, if a portmap service (either external or
		/// embedded) is running and can be contacted.
		/// </returns>
		public static bool isPortmapRunning(int checkTimeout)
		{
			bool available = false;
			try
			{
				OncRpcPortmapClient portmap = new OncRpcPortmapClient(IPAddress.Loopback);
				portmap.getOncRpcClient().setTimeout(checkTimeout);
				portmap.ping();
				available = true;
			}
			catch (OncRpcException)
			{
                // We get noise from here if the portmapper is down
                //Console.Out.WriteLine(e.Message);
                //Console.Out.WriteLine(e.StackTrace);
            }
			catch (IOException)
			{
                // We get noise from here if the portmapper is down
                //Console.Out.WriteLine(e.Message);
                //Console.Out.WriteLine(e.StackTrace);
            }
			return available;
		}

		/// <summary>Indicates whether the embedded portmap service is in use.</summary>
		/// <remarks>Indicates whether the embedded portmap service is in use.</remarks>
		/// <returns>
		/// <code>true</code>, if embedded portmap service is currently
		/// used.
		/// </returns>
		public virtual bool embeddedPortmapInUse()
		{
			return embeddedPortmap.serviceThread != null;
		}

		/// <summary>Returns the thread object running the embedded portmap service.</summary>
		/// <remarks>Returns the thread object running the embedded portmap service.</remarks>
		/// <returns>
		/// Thread object or <code>null</code> if no embedded portmap
		/// service has been started.
		/// </returns>
		public virtual Thread getEmbeddedPortmapServiceThread()
		{
			return embeddedPortmap.serviceThread;
		}

		/// <summary>Returns object implementing the embedded portmap service.</summary>
		/// <remarks>Returns object implementing the embedded portmap service.</remarks>
		/// <returns>
		/// Embedded portmap object or <code>null</code> if no
		/// embedded portmap service has been started.
		/// </returns>
		public virtual jportmap getEmbeddedPortmap()
		{
			return embeddedPortmap;
		}

		/// <summary>Stop the embedded portmap service if it is running.</summary>
		/// <remarks>
		/// Stop the embedded portmap service if it is running. Normaly you should
		/// not use this method except you need to force the embedded portmap
		/// service to terminate. Under normal conditions the thread responsible
		/// for the embedded portmap service will terminate automatically after the
		/// last ONC/RPC program has been deregistered.
		/// <p>This method
		/// just signals the portmap thread to stop processing ONC/RPC portmap
		/// calls and to terminate itself after it has cleaned up after itself.
		/// </remarks>
		public virtual void shutdown()
		{
			OncRpcServerStub portmap = embeddedPortmap;
			if (portmap != null)
			{
				portmap.stopRpcProcessing();
			}
		}

		/// <summary>
		/// Portmap object acting as embedded portmap service or <code>null</code>
		/// if no embedded portmap service is necessary because the operating
		/// system already supplies one or another portmapper is already running.
		/// </summary>
		/// <remarks>
		/// Portmap object acting as embedded portmap service or <code>null</code>
		/// if no embedded portmap service is necessary because the operating
		/// system already supplies one or another portmapper is already running.
		/// </remarks>
		internal OncRpcEmbeddedPortmap.embeddedjportmap embeddedPortmap;

		/// <summary>References thread object running the embedded portmap service.</summary>
		/// <remarks>References thread object running the embedded portmap service.</remarks>
		internal OncRpcEmbeddedPortmapThread embeddedPortmapThread;

		/// <summary>
		/// Extend the portmap service so that it automatically takes itself out
		/// of service when the last ONC/RPC programs is deregistered.
		/// </summary>
		/// <remarks>
		/// Extend the portmap service so that it automatically takes itself out
		/// of service when the last ONC/RPC programs is deregistered.
		/// </remarks>
		public class embeddedjportmap : org.acplt.oncrpc.apps.jportmap.jportmap
		{
			/// <summary>Creates a new instance of an embeddable portmap service.</summary>
			/// <remarks>Creates a new instance of an embeddable portmap service.</remarks>
			/// <exception cref="System.IO.IOException"></exception>
			/// <exception cref="org.acplt.oncrpc.OncRpcException"></exception>
			public embeddedjportmap(OncRpcEmbeddedPortmap _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <summary>Thread running the embedded portmap service.</summary>
			/// <remarks>Thread running the embedded portmap service.</remarks>
			public Thread serviceThread;

			/// <summary>
			/// Deregister all port settings for a particular (program, version) for
			/// all transports (TCP, UDP, ...).
			/// </summary>
			/// <remarks>
			/// Deregister all port settings for a particular (program, version) for
			/// all transports (TCP, UDP, ...). This method basically falls back to
			/// the implementation provided by the <code>jrpcgen</code> superclass,
			/// but checks whether there are other ONC/RPC programs registered. If
			/// not, it signals itself to shut down the portmap service.
			/// </remarks>
			/// <param name="params">
			/// (program, version) to deregister. The protocol and port
			/// fields are not used.
			/// </param>
			/// <returns><code>true</code> if deregistration succeeded.</returns>
			internal override XdrBoolean unsetPort(OncRpcServerIdent @params)
			{
				XdrBoolean ok = base.unsetPort(@params);
				if (ok.booleanValue())
				{
					//
					// Check for registered programs other than PMAP_PROGRAM.
					//
					bool onlyPmap = true;
					int size = this.servers.Count;
					for (int idx = 0; idx < size; ++idx)
					{
						if (((OncRpcServerIdent)this.servers[idx]).program != jportmap.PMAP_PROGRAM)
						{
							onlyPmap = false;
							break;
						}
					}
					//
					// If only portmap-related entries are left, then shut down this
					// portmap service.
					//
					if (onlyPmap && (this.serviceThread != null))
					{
						this.stopRpcProcessing();
					}
				}
				return ok;
			}

			private readonly OncRpcEmbeddedPortmap _enclosing;
		}

		/// <summary>
		/// The class <code>OncRpcEmbeddedPortmapThread</code> implements a thread
		/// which will run an embedded portmap service.
		/// </summary>
		/// <remarks>
		/// The class <code>OncRpcEmbeddedPortmapThread</code> implements a thread
		/// which will run an embedded portmap service.
		/// </remarks>
		public class OncRpcEmbeddedPortmapThread 
		{
			/// <summary>
			/// Construct a new embedded portmap service thread and associate
			/// it with the portmap object to be used as the service.
			/// </summary>
			/// <remarks>
			/// Construct a new embedded portmap service thread and associate
			/// it with the portmap object to be used as the service. The service
			/// is not started yet.
			/// </remarks>
			public OncRpcEmbeddedPortmapThread(OncRpcEmbeddedPortmap _enclosing, OncRpcEmbeddedPortmap.embeddedjportmap
				 portmap)
			{
				this._enclosing = _enclosing;
				this.portmap = portmap;
			}

			/// <summary>
			/// Run the embedded portmap service thread, starting dispatching
			/// of all portmap transports until we get the signal to shut down.
			/// </summary>
			/// <remarks>
			/// Run the embedded portmap service thread, starting dispatching
			/// of all portmap transports until we get the signal to shut down.
			/// </remarks>
			public void run()
			{
				try
				{
					this.portmap.run(this.portmap.transports);
                    // This is not optimal but we need enough time after we remove the entry
                    // from the portmap to respond ok to the client and I haven't figured out
                    // any better way yet.
                    Thread.Sleep(1000);
                    this.portmap.Close(this.portmap.transports);
                    this.portmap.serviceThread = null;
                }
				catch (Exception e)
				{
                    Console.Out.WriteLine(e.Message);
                    Console.Out.WriteLine(e.StackTrace);
				}
			}

			/// <summary>The embedded portmap service object this thread belongs to.</summary>
			/// <remarks>
			/// The embedded portmap service object this thread belongs to. The
			/// service object implements the ONC/RPC dispatcher and the individual
			/// remote procedures for a portmapper).
			/// </remarks>
			private OncRpcEmbeddedPortmap.embeddedjportmap portmap;

			private readonly OncRpcEmbeddedPortmap _enclosing;
		}
	}
}
