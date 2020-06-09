import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-add-api-resource',
  templateUrl: './add-api-resource.component.html',
  styleUrls: ['./add-api-resource.component.scss']
})
export class AddApiResourceComponent implements OnInit {

  constructor(private translate: TranslateService) { }

  ngOnInit(): void {
  }

}
