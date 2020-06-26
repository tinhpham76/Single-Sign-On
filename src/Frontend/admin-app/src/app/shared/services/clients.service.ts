import { Injectable } from "@angular/core";
import { BaseService } from './base.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '@environments/environment';
import { catchError, map } from 'rxjs/operators';
import { Pagination } from '../models/pagination.model';
import { Client } from '../models/client.model';

@Injectable({ providedIn: 'root' })
export class ClientServices extends BaseService {
    private _sharedHeaders = new HttpHeaders();
    constructor(private http: HttpClient) {
        super();
        this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
    }
    add(entity: any) {
        return this.http.post(`${environment.apiUrl}/api/clients`, JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    getDetail(id) {
        return this.http.get(`${environment.apiUrl}/api/clients/${id}`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    getAllPaging(filter, pageIndex, pageSize) {
        return this.http.get<Pagination<Client>>(`${environment.apiUrl}/api/clients/filter?filter=${filter}&pageIndex=${pageIndex}&pageSize=${pageSize}`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    delete(id) {
        return this.http.delete(`${environment.apiUrl}/api/clients/${id}`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    // Setting basic
    getBasic(id) {
        return this.http.get(`${environment.apiUrl}/api/clients/${id}/basics`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    putBasic(id, entity: any) {
        return this.http.put(`${environment.apiUrl}/api/clients/${id}/basics`
            , JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    postBasicOrigin(id, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/clients/${id}/basics/origins`
            , JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    deleteBasicOrigin(id, originName) {
        return this.http.delete(`${environment.apiUrl}/api/clients/${id}/basics/origins/${originName}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    // Setting setting
    getAllScope(){
        return this.http.get(`${environment.apiUrl}/api/clients/allScopes`
        , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }

    getSetting(id) {
        return this.http.get(`${environment.apiUrl}/api/clients/${id}/settings`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    putSetting(id, entity: any) {
        return this.http.put(`${environment.apiUrl}/api/clients/${id}/settings`
            , JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    postSettingScope(id, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/clients/${id}/settings/scopes`
            , JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    deleteSettingScope(id, scopeName) {
        return this.http.delete(`${environment.apiUrl}/api/clients/${id}/settings/scopes/${scopeName}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    postSettingRedirectUri(id, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/clients/${id}/settings/redirectUris`
            , JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    deleteSettingRedirectUri(id, redirectUriName) {
        return this.http.delete(`${environment.apiUrl}/api/clients/${id}/settings/redirectUris/${redirectUriName}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    postSettingGrantType(id, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/clients/${id}/settings/grantTypes`
            , JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    deleteSettingGrantType(id, grantTypeName) {
        return this.http.delete(`${environment.apiUrl}/api/clients/${id}/settings/grantTypes/${grantTypeName}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    getClientSecret(name: string) {
        return this.http.get(`${environment.apiUrl}/api/clients/${name}/clientSecrets`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addClientSecret(name: string, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/clients/${name}/clientSecrets`,
            JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    deleteClientSecret(name: string, secretId: number) {
        return this.http.delete(`${environment.apiUrl}/api/clients/${name}/clientSecrets/${secretId}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    // Setting authentication
    getAuthentication(id) {
        return this.http.get(`${environment.apiUrl}/api/clients/${id}/Authentications`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    putAuthentication(id, entity: any) {
        return this.http.put(`${environment.apiUrl}/api/clients/${id}/Authentications`
            , JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    postPostLogoutRedirectUris(id, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/clients/${id}/Authentications/postLogoutRedirectUris`
            , JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    deletePostLogoutRedirectUris(id, postLogoutRedirectUris) {
        return this.http.delete(`${environment.apiUrl}/api/clients/${id}/Authentications/postLogoutRedirectUris/${postLogoutRedirectUris}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    // Setting Token
    getToken(id) {
        return this.http.get(`${environment.apiUrl}/api/clients/${id}/Tokens`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    putToken(id, entity: any) {
        return this.http.put(`${environment.apiUrl}/api/clients/${id}/Tokens`
            , JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    // Setting device flow
    getDeviceFlow(id) {
        return this.http.get(`${environment.apiUrl}/api/clients/${id}/deviceFlows`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    putDeviceFlow(id, entity: any) {
        return this.http.put(`${environment.apiUrl}/api/clients/${id}/deviceFlows`
            , JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
}