import { Injectable } from "@angular/core";
import { BaseService } from './base.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '@environments/environment';
import { catchError, map } from 'rxjs/operators';
import { Pagination } from '../models/pagination.model';
import { ApiScope } from '../models/api-scope.model';

@Injectable({ providedIn: 'root' })
export class ApiScopeServices extends BaseService {
    private _sharedHeaders = new HttpHeaders();
    constructor(private http: HttpClient) {
        super();
        this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
    }
    add(entity: ApiScope) {
        return this.http.post(`${environment.apiUrl}/api/ApiScopes`, JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    update(name: string, entity: ApiScope) {
        return this.http.put(`${environment.apiUrl}/api/ApiScopes/${name}`,
            JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    getDetail(name: string) {
        return this.http.get<ApiScope>(`${environment.apiUrl}/api/ApiScopes/${name}`,
            { headers: this._sharedHeaders }).pipe(catchError(this.handleError));

    }
    getAllPaging(filter, pageIndex, pageSize) {
        return this.http.get<Pagination<ApiScope>>(`${environment.apiUrl}/api/ApiScopes/filter?filter=${filter}&pageIndex=${pageIndex}&pageSize=${pageSize}`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    delete(name: string) {
        return this.http.delete(`${environment.apiUrl}/api/ApiScopes/${name}`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }

    //
    getApiResourceClaims(name: string) {
        return this.http.get(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceClaims`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addApiResourceClaim(name: string, entity: ApiScope) {
        return this.http.post(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceClaims`,
            JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    deleteApiResourceClaim(name: string, type: string) {
        return this.http.delete(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceClaims/${type}`,
            { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    getApiResourceScope(name: string) {
        return this.http.get(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceScopes`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addApiResourceScope(name: string, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceScopes`,
            JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    deleteApiResourceScope(name: string, scopeName: string) {
        return this.http.delete(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceScopes/${scopeName}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    getApiResourceSecret(name: string) {
        return this.http.get(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceSecrets`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addApiResourceSecret(name: string, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceSecrets`,
            JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    deleteApiResourceSecret(name: string, secretId: number) {
        return this.http.delete(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceSecrets/${secretId}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    getApiResourceProperty(name: string) {
        return this.http.get(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceProperties`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addApiResourceProperty(name: string, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceProperties`,
            JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    deleteApiResourceProperty(name: string, propertyKey: string) {
        return this.http.delete(`${environment.apiUrl}/api/ApiResources/${name}/apiResourceProperties/${propertyKey}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
}