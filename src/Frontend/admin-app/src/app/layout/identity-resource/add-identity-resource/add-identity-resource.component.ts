import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-add-identity-resource',
  templateUrl: './add-identity-resource.component.html',
  styleUrls: ['./add-identity-resource.component.scss']
})
export class AddIdentityResourceComponent implements OnInit {

  constructor(private translate: TranslateService) { }

  ngOnInit(): void {
  }

}
