   M S B A M L   `   `   `  ����B  >BankApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:BankAppBankApp  5   	       BankApp.AccountInfo   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:BankApp     5   	     � Title$  AccountInfo��5   	   $	��450��6   $	��800��6*   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $	��60* �6   �� 5      $
��140* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��
Сумма��6!   60   �� 5   
   $��1��6   $��Дата открытия��6!   68   �� 5   
   $��2��6   $��Дата закрытия��6!   68   �� 5   
   $��3��6   $��
Вклад��6!   60   �� 5   
   $��0��6)   $��1��66    �� 	MaxLength$ 40��6F   {�.  ��6   �� *
Сумма+6i   �� 5   
   $��1��61   $��1��6>   {�.  ��6   �� *Дата_открытия+6b   �� 5   
   $��2��61   $��1��6>   {�.  ��6   �� *Дата_закрытия+6b   �� 5   
   $��3��67   $��1��6D   {�.  ��6   �� *'%Вклады.Наименование+6h   �� 5   
   $�� ��6   $��4��6   $��0��6'   69   �� 5!   
    �� Orientation$ 
Horizontal=�6   $��4��6.   $��1��6;   .+�5"      �� -   5         ��Name$ SaveBtn��5"      $��Пополнить��6<   6R   �� 5#      -   5        $ 	FilterBtn��5#      $��
Снять��6@   6R   �� 5$      -   5        $ 	CancelBtn��5$      $��Закрыть��6A   6U   5%      5&      5'      