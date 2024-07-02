import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { FlatTreeControl } from '@angular/cdk/tree';
import {
  MatTreeFlatDataSource,
  MatTreeFlattener,
} from '@angular/material/tree';
import { ApplicationService } from '../../../services/common/models/application.service';
import { DialogService } from '../../../services/common/dialog.service';
import { AuthorizeMenuDialogComponent } from '../../../dialogs/authorize-menu-dialog/authorize-menu-dialog.component';

interface ITreeMenu {
  name?: string;
  actions?: ITreeMenu[];
  code?: string;
}

/** Flat node with expandable and level information */
interface ExampleFlatNode {
  expandable: boolean;
  name: string;
  code?: string;
  level: number;
}

@Component({
  selector: 'app-authorize-menu',
  templateUrl: './authorize-menu.component.html',
  styleUrl: './authorize-menu.component.scss',
})
export class AuthorizeMenuComponent extends BaseComponent implements OnInit {
  constructor(
    spinner: NgxSpinnerService,
    private applicationSerice: ApplicationService,
    private dialogService: DialogService
  ) {
    super(spinner);
  }

  async ngOnInit() {
    this.dataSource.data = await (
      await this.applicationSerice.getAuthorizeDefinitioonEndpoints()
    ).map((m) => {
      const treeMenu: ITreeMenu = {
        name: m.name,
        actions: m.actions.map((a) => {
          const _treeMenu: ITreeMenu = {
            name: a.definition,
            code: a.code,
          };
          return _treeMenu;
        }),
      };
      return treeMenu;
    });
  }

  treeControl = new FlatTreeControl<ExampleFlatNode>(
    (node) => node.level,
    (node) => node.expandable
  );

  treeFlattener = new MatTreeFlattener(
    (menu: ITreeMenu, level: number) => {
      return {
        expandable: menu.actions?.length > 0,
        name: menu.name,
        level: level,
        code: menu.code,
      };
    },
    (menu) => menu.level,
    (menu) => menu.expandable,
    (menu) => menu.actions
  );

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  hasChild = (_: number, node: ExampleFlatNode) => node.expandable;

  assignRole(code: string, name: string) {
    this.dialogService.openDialog({
      componentType: AuthorizeMenuDialogComponent,
      data: { code: code, name: name },
      options: {
        width: '750px',
      },
      afterClosed: () => {},
    });
  }
}
