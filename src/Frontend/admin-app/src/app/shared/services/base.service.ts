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
                    errorMessage = `ERROR CONNECT SEVER `;
                    break;
                }
                case 400: {
                    errorMessage = ` Bad Request`;
                    break;
                }
                case 401: {
                    errorMessage = ` Unauthorized`;
                    break;
                } case 402: {
                    errorMessage = ` Payment Required `;
                    break;
                }
                case 403: {
                    errorMessage = ` Forbidden `;
                    break;
                }
                case 404: {
                    errorMessage = ` Not Found `;
                    break;
                }
                case 405: {
                    errorMessage = ` Method Not Allowed `;
                    break;
                }
                case 406: {
                    errorMessage = ` Not Acceptable `;
                    break;
                }
                case 407: {
                    errorMessage = ` Proxy Authentication Required`;
                    break;
                }
                case 408: {
                    errorMessage = ` Request Timeout`;
                    break;
                }
                case 409: {
                    errorMessage = ` Conflict `;
                    break;
                }
                case 410: {
                    errorMessage = ` Gone `;
                    break;
                }
                case 411: {
                    errorMessage = ` Length Required `;
                    break;
                }
                case 412: {
                    errorMessage = ` Precondition Failed `;
                    break;
                }
                case 413: {
                    errorMessage = ` Payload Too Large `;
                    break;
                }
                case 414: {
                    errorMessage = ` URI Too Long `;
                    break;
                }
                case 415: {
                    errorMessage = ` Unsupported Media Type `;
                    break;
                }
                case 416: {
                    errorMessage = ` Range Not Satisfiable `;
                    break;
                }
                case 418: {
                    errorMessage = ` I'm a teapot `;
                    break;
                }
                case 421: {
                    errorMessage = ` Misdirected Request `;
                    break;
                }
                case 422: {
                    errorMessage = ` Unprocessable Entity (WebDAV) `;
                    break;
                }
                case 423: {
                    errorMessage = ` Locked (WebDAV) `;
                    break;
                }
                case 424: {
                    errorMessage = ` Failed Dependency `;
                    break;
                }
                case 425: {
                    errorMessage = ` Too Early `;
                    break;
                }
                case 426: {
                    errorMessage = ` Upgrade Required `;
                    break;
                }
                case 428: {
                    errorMessage = ` Precondition Required `;
                    break;
                }
                case 429: {
                    errorMessage = ` Too Many Requests `;
                    break;
                }
                case 431: {
                    errorMessage = ` Request Header Fields Too Large `;
                    break;
                }
                case 451: {
                    errorMessage = ` Unavailable For Legal Reasons `;
                    break;
                }

                // Server
                case 500: {
                    errorMessage = ` Internal Server Error `;
                    break;
                }
                case 501: {
                    errorMessage = ` Not Implemented `;
                    break;
                }
                case 502: {
                    errorMessage = ` Bad Gateway `;
                    break;
                }
                case 503: {
                    errorMessage = ` Service Unavailable `;
                    break;
                }
                case 504: {
                    errorMessage = ` Gateway Timeout `;
                    break;
                }
                case 505: {
                    errorMessage = ` HTTP Version Not Supported `;
                    break;
                }
                case 506: {
                    errorMessage = ` Variant Also Negotiates `;
                    break;
                }
                case 507: {
                    errorMessage = ` Insufficient Storage (WebDAV) `;
                    break;
                }
                case 508: {
                    errorMessage = ` Loop Detected (WebDAV) `;
                    break;
                }
                case 510: {
                    errorMessage = ` Not Extended `;
                    break;
                }
                case 511: {
                    errorMessage = ` Network Authentication Required `;
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