   M S B A M L   `   `   `  ����B  >BankApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:BankAppBankApp  5   	   !    BankApp.AuthorizationWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:BankApp     5   	     � Title$  AuthorizationWindow��5   	   $	��450��6%   $	��800��62    � Icon$ 	/user.png��6>   .��5	      � .+�5
   
   � $��Center=�6   $	��347��6,   $��Center=�69   $	��260��6T   .+�5      �� $��Login��6   $
��Left=�6$   $	��Top=�6?   $	��135��6W   $��Center=�6c   �� 5      $��Password��6   $
��Left=�6'   $��0,44,0,0q�6B   $	��Top=�6T   $	��135��6l   $��Center=�6x   �� 5      -   5         ��Name$ loginTB��5      $
��Left=�6'   $��	140,0,0,0q�6B    �� TextWrapping$
 Wrap=�6U   $	��Top=�6i   $	��120��6�   $��26��6�   G� 5      -   5         G�Name$ 
passwordTB��5      $
��Left=�6.   $��
140,44,0,0q�6I   $	��Top=�6]   $	��120��6u   $��26��6�   �� 5      -   5         ��Name$ enterButton��5      $��Enter��6*   $��Center=�6:   $��	0,243,0,0q�6W   $	��Top=�6j   $��46��6�   $	��120��6�   6�   �� 5      -   5         ��Name$ captchaImage��5      $
��Left=�6*   $��72��6E   $��0,86,0,0q�6Q   $	��Top=�6c   $	��257��6{   $
��Fill=�6�   �� 5      -   5        $ 	captchaTB��5      $
��Left=�6)   $
 Wrap=�6D   $	��Top=�6X   $	��120��6p   $��26��6|   $��
10,180,0,0q�6�   $y�Hidden=�6�   �� 5      -   5        $ captchaButton��5      $��
Regenerate��6,   $
��Left=�6A   $��150,180,0,0q�6\   $	��Top=�6q   $��26��6�   $	��100��6�   $y�Hidden=�6�   �� 5      -   5         ��Name$ captchaTimerLB��5      $�� ��6,   $
��Left=�67   $	��Top=�6R   $��
42,212,0,0q�6j   $	��185��6~   $��Center=�6�   �� 5      -   5        $ 	regButton��5      $��Registration��6(   $��Center=�6?   $��	0,294,0,0q�6\   $	��Top=�6o   $��46��6�   $	��120��6�   6�   5      5      5      