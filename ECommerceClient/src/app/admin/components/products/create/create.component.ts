import { Component, EventEmitter, Output } from '@angular/core';
import { ProductService } from '../../../../services/common/models/product.service';
import { Create_Product } from '../../../../contracts/create_product';
import { BaseComponent, SpinnerType } from '../../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import {
  AlertifyService,
  MessageType,
  Position,
} from '../../../../services/admin/alertify.service';
import { error } from 'console';
import { FileUploadOptions } from '../../../../services/common/file-upload/file-upload.component';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrl: './create.component.scss',
})
export class CreateComponent extends BaseComponent {
  constructor(
    spinner: NgxSpinnerService,
    private productService: ProductService,
    private alertify: AlertifyService
  ) {
    super(spinner);
  }

  ngOnInit(): void {}

  @Output() creaatedProduct: EventEmitter<Create_Product> = new EventEmitter();

  create(
    name: HTMLInputElement,
    stock: HTMLInputElement,
    price: HTMLInputElement
  ) {
    this.showSpinner(SpinnerType.BallAtom);
    const product_create: Create_Product = new Create_Product();
    product_create.name = name.value;
    product_create.stock = parseInt(stock.value);
    product_create.price = parseFloat(price.value);

    this.productService.create(
      product_create,
      () => {
        this.hideSpinner(SpinnerType.BallAtom);
        this.alertify.message('Product has been added successfully', {
          dismissOthers: true,
          messageType: MessageType.Success,
          position: Position.TopRight,
        });

        this.creaatedProduct.emit(product_create);
      },
      (errorMessage) => {
        this.alertify.message(errorMessage, {
          dismissOthers: true,
          messageType: MessageType.Error,
          position: Position.TopRight,
        });
      }
    );
  }
}
