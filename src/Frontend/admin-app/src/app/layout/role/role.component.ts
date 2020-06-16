import { Component, OnInit, OnDestroy, ErrorHandler } from '@angular/core';
import { RolesServices } from '@app/shared/services/role.service';
import { Subscription, EMPTY, throwError } from 'rxjs';
import { Pagination } from '@app/shared/models/pagination.model';
import { Role } from '@app/shared/models/role.model';
import { map, catchError } from 'rxjs/operators';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { NzNotificationService, NzNotificationPlacement } from 'ng-zorro-antd/notification';
import { NzModalService, NzModalRef } from 'ng-zorro-antd/modal';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.scss']
})
export class RoleComponent implements OnInit, OnDestroy {

  // load role data
  filter = '';
  pageIndex = 1;
  pageSize = 5;
  items: any[];
  totalRecords: number;

  // button cofirmModal
  confirmModal?: NzModalRef;

  // load role detail
  id: string;
  name: string;
  normalizedName: string;

  // show edit role
  visibleEditRole = false;
  visibleAddRole = false;

  // Form
  validateForm!: FormGroup;

  // Spin
  isSpinningRole = true;
  isSpinningEditRole = true;
  isSpinningAddRole = true;

  constructor(private rolesService: RolesServices,
    private notification: NzNotificationService,
    private modal: NzModalService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      id: [null, [Validators.required]],
      name: [null, [Validators.required]]
    });
    this.loadRoleData(this.filter, this.pageIndex, this.pageSize);
  }
  ngOnDestroy(): void {
  }

  // Load Role Data
  loadRoleData(filter: string, pageIndex: number, pageSize: number): void {
    this.rolesService.getAllPaging(filter, pageIndex, pageSize)
      .subscribe(res => {
        this.items = res.items;
        this.totalRecords = res.totalRecords;
        setTimeout(() => {
          this.isSpinningRole = false;
        }, 1000);
      }, error => {
        setTimeout(() => {
          this.isSpinningRole = false;
        }, 1000);
      });
  }

  // Delete Role
  delete(id) {
    this.rolesService.delete(id)
      .subscribe(res => {
        this.createNotification('success', 'SSO Admin', 'Delete success ' + id + ' !', 'bottomRight');
        this.ngOnInit();
      });
  }

  showConfirm(roleId: string): void {
    this.confirmModal = this.modal.confirm({
      nzTitle: 'Do you Want to delete ' + roleId + ' role ?',
      nzContent: 'When clicked the OK button, this dialog will be closed after 1 second',
      nzOnOk: () =>
        new Promise((resolve, reject) => {
          this.delete(roleId);
          setTimeout(Math.random() > 0.5 ? resolve : reject, 200);
        })
    });
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageSize, pageIndex } = params;
    this.loadRoleData(this.filter, pageIndex, pageSize);
  }

  createNotification(type: string, title: string, content: string, position: NzNotificationPlacement): void {
    this.notification.create(type, title, content, { nzPlacement: position });
  }

  openEditRole(roleId: string): void {
    this.visibleEditRole = true;
    this.rolesService.getDetail(roleId)
      .subscribe((res: Role) => {
        this.validateForm.setValue({
          id: res.id,
          name: res.name
        });
        setTimeout(() => {
          this.isSpinningEditRole = false;
        }, 500);
      }, error => {
        setTimeout(() => {
          this.isSpinningEditRole = false;
        }, 500);
      });
  }
  openAddRole(): void {
    this.visibleEditRole = true;
    this.isSpinningAddRole = true;
  }

  closeEditRole(): void {
    this.visibleEditRole = false;
    this.isSpinningEditRole = true;
  }
  closeAddRole(): void {
    this.visibleAddRole = false;
    this.isSpinningAddRole = true;
  }


  saveChanges(): void {
    const id = this.validateForm.get('id').value;
    const data = this.validateForm.getRawValue();
    this.rolesService.update(id, data)
      .subscribe(() => {
        this.createNotification('success', 'SSO Admin', 'Update success ' + id + '!', 'bottomRight');
        setTimeout(() => {
          this.closeEditRole();
          this.ngOnInit();
        }, 500);
      },
      error => {
        setTimeout(() => {
          this.isSpinningEditRole = false;
        }, 500);
      });
  }
}
