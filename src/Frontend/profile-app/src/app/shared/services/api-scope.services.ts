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
    getApiScopeClaims(name: string) {
        return this.http.get(`${environment.apiUrl}/api/ApiScopes/${name}/scopeClaims`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addApiScopeClaim(name: string, entity: ApiScope) {
        return this.http.post(`${environment.apiUrl}/api/ApiScopes/${name}/scopeClaims`,
            JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    deleteApiScopeClaim(name: string, type: string) {
        return this.http.delete(`${environment.apiUrl}/api/ApiScopes/${name}/scopeClaims/${type}`,
            { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    getApiScopeProperty(name: string) {
        return this.http.get(`${environment.apiUrl}/api/ApiScopes/${name}/scopeProperties`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addApiScopeProperty(name: string, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/ApiScopes/${name}/scopeProperties`,
            JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    deleteApiScopeProperty(name: string, propertyKey: string) {
        return this.http.delete(`${environment.apiUrl}/api/ApiScopes/${name}/scopeProperties/${propertyKey}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
}