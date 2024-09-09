import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';
import { Inject, Injectable, InjectionToken, Optional } from '@angular/core';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';

export const API_BASE_URL = new InjectionToken<string>('API_BASE_URL');


export interface IAccountsClient {
  login(command: LoginRequestCommand): Observable<AuthenticatedResponse>;
  refreshToken(): Observable<AuthenticatedResponse>;
}

@Injectable()
export class AccountsClient implements IAccountsClient {
  private http: HttpClient;
  private baseUrl: string;
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

  constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
    this.http = http;
    this.baseUrl = baseUrl ?? "";
  }

  login(command: LoginRequestCommand): Observable<AuthenticatedResponse> {
    let url_ = this.baseUrl + "/api/Accounts/Login";
    url_ = url_.replace(/[?&]$/, "");

    const content_ = JSON.stringify(command);

    let options_: any = {
      body: content_,
      observe: "response",
      responseType: "blob",
      withCredentials: true,
      headers: new HttpHeaders({
        "Content-Type": "application/json",
        "Accept": "application/json"
      })
    };

    return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_: any) => {
      return this.processLogin(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processLogin(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<AuthenticatedResponse>;
        }
      } else
        return _observableThrow(response_) as any as Observable<AuthenticatedResponse>;
    }));
  }

  protected processLogin(response: HttpResponseBase): Observable<AuthenticatedResponse> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        let result200: any = null;
        let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result200 = AuthenticatedResponse.fromJS(resultData200);
        return _observableOf(result200);
      }));
    } else if (status === 400) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        let result400: any = null;
        let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result400 = ProblemDetails.fromJS(resultData400);
        return throwException("A server side error occurred.", status, _responseText, _headers, result400);
      }));
    } else if (status === 404) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        return throwException("A server side error occurred.", status, _responseText, _headers);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf(null as any);
  }

  refreshToken(): Observable<AuthenticatedResponse> {
    let url_ = this.baseUrl + "/api/Accounts/RefreshToken";
    url_ = url_.replace(/[?&]$/, "");

    let options_: any = {
      observe: "response",
      responseType: "blob",
      withCredentials: true,
      headers: new HttpHeaders({
        "Accept": "application/json"
      })
    };

    return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_: any) => {
      return this.processRefreshToken(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processRefreshToken(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<AuthenticatedResponse>;
        }
      } else
        return _observableThrow(response_) as any as Observable<AuthenticatedResponse>;
    }));
  }

  protected processRefreshToken(response: HttpResponseBase): Observable<AuthenticatedResponse> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        let result200: any = null;
        let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result200 = AuthenticatedResponse.fromJS(resultData200);
        return _observableOf(result200);
      }));
    } else if (status === 400) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        let result400: any = null;
        let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result400 = ProblemDetails.fromJS(resultData400);
        return throwException("A server side error occurred.", status, _responseText, _headers, result400);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf(null as any);
  }
  logout(): Observable<void> {
    let url_ = this.baseUrl + "/api/Accounts/Logout";
    url_ = url_.replace(/[?&]$/, "");

    let options_: any = {
      observe: "response",
      responseType: "blob",
      withCredentials: true,
      headers: new HttpHeaders({
      })
    };

    return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_: any) => {
      return this.processLogout(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processLogout(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<void>;
        }
      } else
        return _observableThrow(response_) as any as Observable<void>;
    }));
  }

  protected processLogout(response: HttpResponseBase): Observable<void> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        return _observableOf(null as any);
      }));
    } else if (status === 400) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        let result400: any = null;
        let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result400 = ProblemDetails.fromJS(resultData400);
        return throwException("A server side error occurred.", status, _responseText, _headers, result400);
      }));
    } else if (status === 404) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        return throwException("A server side error occurred.", status, _responseText, _headers);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf(null as any);
  }

  changePassword(command: ChangePasswordCommand): Observable<void> {
    let url_ = this.baseUrl + "/api/Accounts/ChangePassword";
    url_ = url_.replace(/[?&]$/, "");

    const content_ = JSON.stringify(command);

    let options_: any = {
      body: content_,
      observe: "response",
      responseType: "blob",
      withCredentials: true,
      headers: new HttpHeaders({
        "Content-Type": "application/json",
      })
    };

    return this.http.request("post", url_, options_).pipe(_observableMergeMap((response_: any) => {
      return this.processChangePassword(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processChangePassword(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<void>;
        }
      } else
        return _observableThrow(response_) as any as Observable<void>;
    }));
  }

  protected processChangePassword(response: HttpResponseBase): Observable<void> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
    if (status === 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        return _observableOf(null as any);
      }));
    } else if (status === 400) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        let result400: any = null;
        let resultData400 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result400 = ProblemDetails.fromJS(resultData400);
        return throwException("A server side error occurred.", status, _responseText, _headers, result400);
      }));
    } else if (status === 404) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        return throwException("A server side error occurred.", status, _responseText, _headers);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf(null as any);
  }
  
  getUserPermissions(allowCache: boolean | undefined): Observable<string[]> {
    let url_ = this.baseUrl + "/api/Accounts/GetUserPermissions";
    url_ = url_.replace(/[?&]$/, "");

    const content_ = JSON.stringify(allowCache);

    let options_: any = {
      body: content_,
      observe: "response",
      responseType: "blob",
      withCredentials: true,
      headers: new HttpHeaders({
        "Content-Type": "application/json",
        "Accept": "application/json"
      })
    };

    return this.http.request("get", url_, options_).pipe(_observableMergeMap((response_: any) => {
      return this.processGetUserPermissions(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processGetUserPermissions(response_ as any);
        } catch (e) {
          return _observableThrow(e) as any as Observable<string[]>;
        }
      } else
        return _observableThrow(response_) as any as Observable<string[]>;
    }));
  }

  protected processGetUserPermissions(response: HttpResponseBase): Observable<string[]> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (response as any).error instanceof Blob ? (response as any).error : undefined;

    let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); } }
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        let result200: any = null;
        let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
        if (Array.isArray(resultData200)) {
          result200 = [] as any;
          for (let item of resultData200)
            result200!.push(item);
        }
        else {
          result200 = <any>null;
        }
        return _observableOf(result200);
      }));
    } else if (status === 404) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        return throwException("A server side error occurred.", status, _responseText, _headers);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      }));
    }
    return _observableOf(null as any);
  }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
  if (result !== null && result !== undefined)
    return _observableThrow(result);
  else
    return _observableThrow(new LMSException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
  return new Observable<string>((observer: any) => {
    if (!blob) {
      observer.next("");
      observer.complete();
    } else {
      let reader = new FileReader();
      reader.onload = event => {
        observer.next((event.target as any).result);
        observer.complete();
      };
      reader.readAsText(blob);
    }
  });

}

export class LMSException extends Error {
  override message: string;
  status: number;
  response: string;
  headers: { [key: string]: any; };
  result: any;

  constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
    super();

    this.message = message;
    this.status = status;
    this.response = response;
    this.headers = headers;
    this.result = result;
  }

  protected isLMSException = true;

  static isLMSException(obj: any): obj is LMSException {
    return obj.isLMSException === true;
  }
}

export class AuthenticatedResponse implements IAuthenticatedResponse {
  accessToken?: string;
  tokenType?: string;
  expiresInMinutes?: number;
  refreshToken?: string;
  refreshTokenExpiresOn?: Date;

  constructor(data?: IAuthenticatedResponse) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(_data?: any) {
    if (_data) {
      this.accessToken = _data["accessToken"];
      this.tokenType = _data["tokenType"];
      this.expiresInMinutes = _data["expiresInMinutes"];
      this.refreshToken = _data["refreshToken"];
      this.refreshTokenExpiresOn = _data["refreshTokenExpiresOn"] ? new Date(_data["refreshTokenExpiresOn"].toString()) : <any>undefined;
    }
  }

  static fromJS(data: any): AuthenticatedResponse {
    data = typeof data === 'object' ? data : {};
    let result = new AuthenticatedResponse();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data["accessToken"] = this.accessToken;
    data["tokenType"] = this.tokenType;
    data["expiresInMinutes"] = this.expiresInMinutes;
    data["refreshToken"] = this.refreshToken;
    data["refreshTokenExpiresOn"] = this.refreshTokenExpiresOn ? this.refreshTokenExpiresOn.toISOString() : <any>undefined;
    return data;
  }
}

export interface IAuthenticatedResponse {
  accessToken?: string;
  tokenType?: string;
  expiresInMinutes?: number;
  refreshToken?: string;
  refreshTokenExpiresOn?: Date;
}

export class LoginRequestCommand implements ILoginRequestCommand {
  userName?: string;
  password?: string;
  isRemember?: boolean;

  constructor(data?: ILoginRequestCommand) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(_data?: any) {
    if (_data) {
      this.userName = _data["userName"];
      this.password = _data["password"];
      this.isRemember = _data["isRemember"];
    }
  }

  static fromJS(data: any): LoginRequestCommand {
    data = typeof data === 'object' ? data : {};
    let result = new LoginRequestCommand();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data["userName"] = this.userName;
    data["password"] = this.password;
    data["isRemember"] = this.isRemember;
    return data;
  }
}

export interface ILoginRequestCommand {
  userName?: string;
  password?: string;
  isRemember?: boolean;
}

export class ProblemDetails implements IProblemDetails {
  type?: string | undefined;
  title?: string | undefined;
  status?: number | undefined;
  detail?: string | undefined;
  instance?: string | undefined;

  [key: string]: any;

  constructor(data?: IProblemDetails) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(_data?: any) {
    if (_data) {
      for (var property in _data) {
        if (_data.hasOwnProperty(property))
          this[property] = _data[property];
      }
      this.type = _data["type"];
      this.title = _data["title"];
      this.status = _data["status"];
      this.detail = _data["detail"];
      this.instance = _data["instance"];
    }
  }

  static fromJS(data: any): ProblemDetails {
    data = typeof data === 'object' ? data : {};
    let result = new ProblemDetails();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    for (var property in this) {
      if (this.hasOwnProperty(property))
        data[property] = this[property];
    }
    data["type"] = this.type;
    data["title"] = this.title;
    data["status"] = this.status;
    data["detail"] = this.detail;
    data["instance"] = this.instance;
    return data;
  }
}

export class ChangePasswordCommand implements IChangePasswordCommand {
  currentPassword?: string;
  newPassword?: string;

  constructor(data?: IChangePasswordCommand) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(_data?: any) {
    if (_data) {
      this.currentPassword = _data["currentPassword"];
      this.newPassword = _data["newPassword"];
    }
  }

  static fromJS(data: any): ChangePasswordCommand {
    data = typeof data === 'object' ? data : {};
    let result = new ChangePasswordCommand();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data["currentPassword"] = this.currentPassword;
    data["newPassword"] = this.newPassword;
    return data;
  }
}

export interface IChangePasswordCommand {
  currentPassword?: string;
  newPassword?: string;
}

export interface IProblemDetails {
  type?: string | undefined;
  title?: string | undefined;
  status?: number | undefined;
  detail?: string | undefined;
  instance?: string | undefined;

  [key: string]: any;
}
