import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { BasketService } from '../../../services/common/models/basket.service';
import { List_Basket_Item } from '../../../contracts/basket/list-basket-item';
import { Update_Basket_Item } from '../../../contracts/basket/update_basket_item';
import { OrderService } from '../../../services/common/models/order.service';
import { Create_Order } from '../../../contracts/order/create_order';
import {
  CustomToastrService,
  ToastrMessageType,
  ToastrPosition,
} from '../../../services/ui/custom-toastr.service';
import { Route, Router } from '@angular/router';
declare var $: any;

@Component({
  selector: 'app-baskets',
  templateUrl: './baskets.component.html',
  styleUrl: './baskets.component.scss',
})
export class BasketsComponent extends BaseComponent implements OnInit {
  constructor(
    spinner: NgxSpinnerService,
    private basketService: BasketService,
    private orderService: OrderService,
    private toastrService: CustomToastrService,
    private router: Router
  ) {
    super(spinner);
  }

  basketItems: List_Basket_Item[];
  async ngOnInit(): Promise<any> {
    this.showSpinner(SpinnerType.BallAtom);
    this.basketItems = await this.basketService.get();
    this.hideSpinner(SpinnerType.BallAtom);
  }

  async changeQuantity(object: any) {
    this.showSpinner(SpinnerType.BallAtom);
    const basketItemId: string = object.target.attributes['id'].value;
    const quantity: number = object.target.value;
    const basketItem: Update_Basket_Item = new Update_Basket_Item();
    basketItem.basketItemId = basketItemId;
    basketItem.quantity = quantity;
    await this.basketService.updateQuantity(basketItem);
    this.hideSpinner(SpinnerType.BallAtom);
  }

  async removeBasketItem(basketItemId: string) {
    this.showSpinner(SpinnerType.BallAtom);

    await this.basketService.remove(basketItemId);
    $('.' + basketItemId).fadeOut(2000, () =>
      this.hideSpinner(SpinnerType.BallAtom)
    );
  }

  async shoppingCompleted() {
    this.showSpinner(SpinnerType.BallAtom);
    const order: Create_Order = new Create_Order();
    order.address = ' Vasmoy';
    order.description = 'Hebele hubele';
    await this.orderService.create(order);
    this.hideSpinner(SpinnerType.BallAtom);
    this.toastrService.message('Order placed!', 'Order created', {
      messageType: ToastrMessageType.Info,
      position: ToastrPosition.TopRight,
    });
    this.router.navigate(['/']);
  }
}
