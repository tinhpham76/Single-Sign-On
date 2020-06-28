import { throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

export abstract class BaseService {

    constructor() { }

    protected handleError(error: HttpErrorResponse) {
        let errorMessage = '';
        if (error.error instanceof ErrorEvent) {
            // client-side error
            errorMessage = `Error: ${error.error.message}`;
        } else {
            // server-side error
            switch (Number(error.status)) {
                case 0: {
                    errorMessage = `ERROR CONNECT SEVER: ${error.error}`;
                    break;
                }
                case 400: {
                    errorMessage = ` Bad Request: ${error.error}`;
                    break;
                }
                case 401: {
                    errorMessage = ` Unauthorized: ${error.error}`;
                    break;
                } case 402: {
                    errorMessage = ` Payment Required: ${error.error}`;
                    break;
                }
                case 403: {
                    errorMessage = ` Forbidden: ${error.error}`;
                    break;
                }
                case 404: {
                    errorMessage = ` Not Found: ${error.error}`;
                    break;
                }
                case 405: {
                    errorMessage = ` Method Not Allowed: ${error.error}`;
                    break;
                }
                case 406: {
                    errorMessage = ` Not Acceptable: ${error.error}`;
                    break;
                }
                case 407: {
                    errorMessage = ` Proxy Authentication Required: ${error.error}`;
                    break;
                }
                case 408: {
                    errorMessage = ` Request Timeout: ${error.error}`;
                    break;
                }
                case 409: {
                    errorMessage = ` Conflict: ${error.error}`;
                    break;
                }
                case 410: {
                    errorMessage = ` Gone: ${error.error}`;
                    break;
                }
                case 411: {
                    errorMessage = ` Length Required: ${error.error}`;
                    break;
                }
                case 412: {
                    errorMessage = ` Precondition Failed: ${error.error}`;
                    break;
                }
                case 413: {
                    errorMessage = ` Payload Too Large: ${error.error}`;
                    break;
                }
                case 414: {
                    errorMessage = ` URI Too Long: ${error.error}`;
                    break;
                }
                case 415: {
                    errorMessage = ` Unsupported Media Type: ${error.error}`;
                    break;
                }
                case 416: {
                    errorMessage = ` Range Not Satisfiable: ${error.error}`;
                    break;
                }
                case 418: {
                    errorMessage = ` I'm a teapot: ${error.error}`;
                    break;
                }
                case 421: {
                    errorMessage = ` Misdirected Request: ${error.error}`;
                    break;
                }
                case 422: {
                    errorMessage = ` Unprocessable Entity (WebDAV): ${error.error}`;
                    break;
                }
                case 423: {
                    errorMessage = ` Locked (WebDAV): ${error.error}`;
                    break;
                }
                case 424: {
                    errorMessage = ` Failed Dependency: ${error.error}`;
                    break;
                }
                case 425: {
                    errorMessage = ` Too Early: ${error.error}`;
                    break;
                }
                case 426: {
                    errorMessage = ` Upgrade Required: ${error.error}`;
                    break;
                }
                case 428: {
                    errorMessage = ` Precondition Required: ${error.error}`;
                    break;
                }
                case 429: {
                    errorMessage = ` Too Many Requests: ${error.error}`;
                    break;
                }
                case 431: {
                    errorMessage = ` Request Header Fields Too Large: ${error.error}`;
                    break;
                }
                case 451: {
                    errorMessage = ` Unavailable For Legal Reasons: ${error.error}`;
                    break;
                }

                // Server
                case 500: {
                    errorMessage = ` Internal Server Error: ${error.error}`;
                    break;
                }
                case 501: {
                    errorMessage = ` Not Implemented: ${error.error}`;
                    break;
                }
                case 502: {
                    errorMessage = ` Bad Gateway: ${error.error}`;
                    break;
                }
                case 503: {
                    errorMessage = ` Service Unavailable: ${error.error}`;
                    break;
                }
                case 504: {
                    errorMessage = ` Gateway Timeout: ${error.error}`;
                    break;
                }
                case 505: {
                    errorMessage = ` HTTP Version Not Supported: ${error.error}`;
                    break;
                }
                case 506: {
                    errorMessage = ` Variant Also Negotiates: ${error.error}`;
                    break;
                }
                case 507: {
                    errorMessage = ` Insufficient Storage (WebDAV): ${error.error}`;
                    break;
                }
                case 508: {
                    errorMessage = ` Loop Detected (WebDAV): ${error.error}`;
                    break;
                }
                case 510: {
                    errorMessage = ` Not Extended: ${error.error}`;
                    break;
                }
                case 511: {
                    errorMessage = ` Network Authentication Required: ${error.error}`;
                    break;
                }

                default: {
                    break;
                }
            }
        }
        console.log(errorMessage);
        return throwError(errorMessage);
    }
}