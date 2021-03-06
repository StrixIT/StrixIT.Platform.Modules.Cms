﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was not generated by a tool. but for stylecop suppression.
// </auto-generated>
//------------------------------------------------------------------------------
using Moq;
using System.Web;

namespace StrixIT.Platform.Modules.Cms.Tests
{
    public class HttpContextMock
    {
        #region Private Fields

        private Mock<HttpContextBase> _context = new Mock<HttpContextBase>();
        private Mock<HttpRequestBase> _request = new Mock<HttpRequestBase>();
        private Mock<HttpResponseBase> _response = new Mock<HttpResponseBase>();
        private Mock<HttpServerUtilityBase> _server = new Mock<HttpServerUtilityBase>();
        private Mock<HttpSessionStateBase> _session = new Mock<HttpSessionStateBase>();

        #endregion Private Fields

        #region Public Constructors

        public HttpContextMock()
        {
            _context.Setup(ct => ct.Request).Returns(_request.Object);
            _context.Setup(ct => ct.Response).Returns(_response.Object);
            _context.Setup(ct => ct.Session).Returns(_session.Object);
            _context.Setup(ct => ct.Server).Returns(_server.Object);
        }

        #endregion Public Constructors

        #region Public Properties

        public HttpContextBase HttpContext
        {
            get
            {
                return _context.Object;
            }
        }

        public Mock<HttpRequestBase> RequestMock
        {
            get
            {
                return _request;
            }
        }

        public Mock<HttpResponseBase> ResponseMock
        {
            get
            {
                return _response;
            }
        }

        public Mock<HttpServerUtilityBase> ServerMock
        {
            get
            {
                return _server;
            }
        }

        public Mock<HttpSessionStateBase> SessionMock
        {
            get
            {
                return _session;
            }
        }

        #endregion Public Properties
    }
}