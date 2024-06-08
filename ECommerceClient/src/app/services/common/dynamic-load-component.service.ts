import {
  ComponentFactoryResolver,
  Injectable,
  ViewContainerRef,
} from '@angular/core';
import { BaseComponent } from '../../base/base.component';

@Injectable({
  providedIn: 'root',
})
export class DynamicLoadComponentService {
  //ViewContainerRef: Dinamik olaraq yuklenecek componenti ozunde saxliyan containerdir.(Her dinamik yuklemeden evvel onceki viewlari clear() etmek lazimdir!)
  //ComponentFactory: Componentlerin instancesini yaratmaq ucun isledilen  objectdir.
  //ComponentFactoryResolver: Mueyyen bir component ucun ComponentFactory ni resolve eden sinifdir. resolveFactoryComponent() funksiyasi ile mueyyen componente aid ComponentFactory objecti yaradib geri qaytarir.

  constructor(private componentFactoryResolver: ComponentFactoryResolver) {}

  async loadComponent(
    component: ComponentType,
    viewContainerRef: ViewContainerRef
  ) {
    let _component: any = null;
    switch (component) {
      case ComponentType.BasketsComponent:
        _component = (
          await import('../../../app/ui/components/baskets/baskets.component')
        ).BasketsComponent;
        break;
    }

    viewContainerRef.clear();
    return viewContainerRef.createComponent(
      this.componentFactoryResolver.resolveComponentFactory(_component)
    );
  }
}

export enum ComponentType {
  BasketsComponent,
}
