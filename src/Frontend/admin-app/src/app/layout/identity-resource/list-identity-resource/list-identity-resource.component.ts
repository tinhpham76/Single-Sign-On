import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-list-identity-resource',
  templateUrl: './list-identity-resource.component.html',
  styleUrls: ['./list-identity-resource.component.scss']
})
export class ListIdentityResourceComponent implements OnInit {

  constructor(private translate: TranslateService) { }

  ngOnInit(): void {
  }

}
