import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BaseService } from './base.service';
import { catchError } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { User } from '../models/user.model';
import { UtilitiesService } from './utilities.service';

@Injectable({ providedIn: 'root' })
export class UserService extends BaseService {
    private _sharedHeaders = new HttpHeaders();
    constructor(private http: HttpClient, private utilitiesService: UtilitiesService) {
        super();
        this._sharedHeaders = this._sharedHeaders.set('Content-Type', 'application/json');

    }
    getAll() {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/json'
            })
        };
        return this.http.get<User[]>(`${environment.apiUrl}/api/users`, httpOptions)
            .pipe(catchError(this.handleError));
    }
    getDetail(id) {
        return this.http.get<User>(`${environment.apiUrl}/api/users/${id}`, { headers: this._sharedHeaders })
            .pipe(catchError(this.handleError));
    }
}