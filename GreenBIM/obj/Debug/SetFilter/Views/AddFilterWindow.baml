   M S B A M L   `   `   `  ����C  ?GreenBIM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:GreenBIM.SetFilter.ViewsGreenBIM.SetFilter.Views  5      K*clr-namespace:GreenBIM.SetFilter.ViewModelGreenBIM.SetFilter.ViewModel  5      .    (GreenBIM.SetFilter.Views.AddFilterWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:GreenBIM.SetFilter.Views     5      9vm*clr-namespace:GreenBIM.SetFilter.ViewModel     5      -   5        $��Window��5      $	��450��5
      $	��320��6   .��5      � $
��Left=�6   $	��300��6&     � RowDefinitions  5   
   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   �� $
��10,5q�6   
���   �$��2q�63    �� CornerRadius$ 2��5      
�������._�5      �� $y��sВыберите фильтры которые будите использовать в текущем сеансе��6   $��Right=�5      $
��Wrap=�6,   $��0��6@    �� TextAlignment$ Center=�5      $��Center=�6+   $��0,2,8,2q�6F   $	��270��6W   5      �� 5   
   $��1��6   $��10q�6   ��6+   �� *AllFiltersList+��5      �� 5      .R�5      �� ��6   �� *
FilterName+ d� 	IsChecked 6>   �� *IsOn+5      5      �� 5    
   $��2��6   $��30��6   $��60��6*   ���#0078D4
�������$��Принять��5#      $��2q�5$      $��10,7,230,13q�5%      ��5!      ��  �� Path$ AddFilterWinApplyCommand���5"      ��  �� ElementName$ Window��5%      �� 5&   
   $��2��5'      $��30��5(      $��60��5)      
�������$��Отмена��5-      $��2q�5.      $��230,7,10,13q�5/      ��5+      �� $ Window����5,      �� $ CancelCommand�5/      51      52      