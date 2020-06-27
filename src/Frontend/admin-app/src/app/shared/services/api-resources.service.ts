import { Injectable } from "@angular/core";
import { BaseService } from './base.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '@environments/environment';
import { catchError, map } from 'rxjs/operators';
import { Pagination } from '../models/pagination.model';
import { ApiResource } from '../models/api-resource.model';

@Injectable({ providedIn: 'root' })
export class ApiResourceServices extends BaseService {
    private _sharedHeaders = new HttpHeaders();
    constructor(private http: HttpClient) {
        super();
        this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');
    }
    add(entity: ApiResource) {
        return this.http.post(`${environment.apiUrl}/api/ApiResources`, JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    update(name: string, entity: ApiResource) {
        return this.http.put(`${environment.apiUrl}/api/ApiResources/${name}`,
            JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    getDetail(name: string) {
        return this.http.get<ApiResource>(`${environment.apiUrl}/api/ApiResources/${name}`,
            { headers: this._sharedHeaders }).pipe(catchError(this.handleError));

    }
    getAllPaging(filter, pageIndex, pageSize) {
        return this.http.get<Pagination<ApiResource>>(`${environment.apiUrl}/api/ApiResources/filter?filter=${filter}&pageIndex=${pageIndex}&pageSize=${pageSize}`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    delete(name: string) {
        return this.http.delete(`${environment.apiUrl}/api/ApiResources/${name}`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    getApiResourceClaims(name: string) {
        return this.http.get(`${environment.apiUrl}/api/ApiResources/${name}/resourceClaims`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addApiResourceClaim(name: string, entity: ApiResource) {
        return this.http.post(`${environment.apiUrl}/api/ApiResources/${name}/resourceClaims`,
            JSON.stringify(entity), { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    deleteApiResourceClaim(name: string, type: string) {
        return this.http.delete(`${environment.apiUrl}/api/ApiResources/${name}/resourceClaims/${type}`,
            { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    getApiResourceScope(name: string) {
        return this.http.get(`${environment.apiUrl}/api/ApiResources/${name}/resourceScopes`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addApiResourceScope(name: string, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/ApiResources/${name}/resourceScopes`,
            JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    deleteApiResourceScope(name: string, scopeName: string) {
        return this.http.delete(`${environment.apiUrl}/api/ApiResources/${name}/resourceScopes/${scopeName}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    getApiResourceSecret(name: string) {
        return this.http.get(`${environment.apiUrl}/api/ApiResources/${name}/resourceSecrets`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addApiResourceSecret(name: string, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/ApiResources/${name}/resourceSecrets`,
            JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    deleteApiResourceSecret(name: string, secretId: number) {
        return this.http.delete(`${environment.apiUrl}/api/ApiResources/${name}/resourceSecrets/${secretId}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
    getApiResourceProperty(name: string) {
        return this.http.get(`${environment.apiUrl}/api/ApiResources/${name}/resourceProperties`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    addApiResourceProperty(name: string, entity: any) {
        return this.http.post(`${environment.apiUrl}/api/ApiResources/${name}/resourceProperties`,
            JSON.stringify(entity), { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
    deleteApiResourceProperty(name: string, propertyKey: string) {
        return this.http.delete(`${environment.apiUrl}/api/ApiResources/${name}/resourceProperties/${propertyKey}`
            , { headers: this._sharedHeaders }).pipe(catchError(this.handleError));
    }
}