[v] Add EFModels
����html�令cshtml

=====
�|�����U/�U�\��(�e�x����100%)-�ݪ��A��Repo
[V]add ���U�|���\��
	add Models/Infra/HashUtility.cs
	add AppSettings,<add key="salt" value="ar!Zu@D691RR"/>
	add Models/ViewModels/RegisterVm.cs
	add RegisterExts class,�X�R��k ToMember(RegisterVM)
	add Controllers/Members/MembersController
		add Register action(Get,Post)
		add Views/Memebrs/Register.cshml(**�s�Wdemo���s), RegisterConfirm.cshml 
	�K�X�]�w�n�D(�����]�t1�ӯS��Ÿ��B���o�֩�8�X)
	
[V]	�o�econfirm email���U�H��
	�w����google email�H�e���U�\��
	!**�ק�Email Helper(SendViaGoogle�令�H�H)
	!**modify MembersController
		edit RegisterMember // �o�e���ҫH
		add GenerateConfirmationLink

[V] ��@ �s�|��Email�T�{�\�� 
	�|���ҥΪ�url: /Members/ActiveRegister?memberId=99&confirmCode=ttttttttttt
	modify MembersController
		add ActiveRegister(memberId, confirmCode)
	add Views/Members/ActiveRegister.cshtml

[V] �K�X�s�W�������

[V] ��@ �n�J�n�X����
	�u���b�K���T�ӥB�w�}�q�|���~���\�n�J�A��@���e�Х��ӧO�إߤw/���}�q���|ĳ����
	
	�ثe�w�����n�J�n�X�\��

	modify web.config, add Authenthcation node
	add Models/ViewModels/LoginVm.cs
	modify MemberController
		add Login action(Get, Post)
		add Logout action(get, only)
	modify _layout,�[�Jlogin,logout link

	Controller �[�J[Authorize] �n�J�~���

[V] ��@ �إ߷|���M��/�s��|�����
	add Models/ViewModels/EditProfileVm.cs
	add Models/ViewModels/MemberExts class, �X�R��k ToEditProfileVm(Member)

	modify MemberController
		add Index action �|���M�ϭ�,�b�n�J���\����|�ɦV����
			add Views/Members/Index.cshtml(�ťսd��)�A���Ҳ�Views/Shared/_mod_membernav.cshtml
		�ק�web.config�̪�defaultUrl="/Members/Index/"
		add EditProfile action(Get,Post)
			����s����
	add MemberExts
	�w���\�s��|�����!

[V]��@�ܧ�K�X
	add Models/ViewModels/EditPasswordVm.cs
	
	modifty MemberController
		add EditPassword action(Get, Post)
		add View/Members/EditPassword.cshtml(��create template)


[V]�ѰO�K�X/���]�K�X
	add Models/ViewModels/ForgotPasswordVm.cs
	modifty MemberController
		add ForgotPassword action(Get, Post)
		add ForgotPassword view Page(create template)
		add Views/Members/ForgotPasswordConfirm.cshtml  view Page(�ťսd��)

		�ק� Views/Members/Login.cshtml �[�J"�ѰO�K�X"�s��

[V] ��@���]�K�X
	add Models/ViewModels/ResetPasswordVm.cs �Ψӭ��s��J�K�X

	modifty MemberController
		add ResetPassword action(Get, Post)
		add Views/Members/ResetPassword.cshtml //���s�]�w�K�X(create)
		add Views/Members/ConfirmResetPassword.cshtml //���s�]�w�K�X�T�{

[V]�j�Y�ӭק�
>�w�g���n�Ǩ��Ƨ�
>�W�ǫݳB�z�A�쥻���������i�H�W�Ǧr��ݳB�z�e�{
�i�W�ǧ�s

��ۥ[JS�g��js��Ƨ�


===============
Cart/Order(60%�A�|���a�J�ϥΪ̸�ƩҥH�ݭn���Ʈw�s�W)

�ʪ��y�{�w�i�H�]���A�]�i�H�s�W��order��Ƥ�

SQL�]���@�w�n�[�J�o���~��B�@����ʪ��y�{!!

-- �Ĥ@����� Carts
INSERT INTO dbo.Carts(MemberAccount, TotalPrice)
VALUES ('allen', 100);


�ثe
Rooms/Roomtype
�o�@�����O�w���h
�u�� / A01�˪L��-�n���b���H�гo�ӥi�H�[�J�ʪ���(�|���]�w���~�T��)
�u���X�F�i�H�[�J�ʪ���!

**�w���\��ݳB�z
[]�k�W�ʪ����[��
[]���~�T��
[]�ɶ��d��
[]�ɶ��P�_

**Cart�\��
[]�ϥεn�J�|���ާ@
[]�W�@�B�s��
[]���~�T��

�q��ĤG�B�I�ڤ覡�|�������X���~�T��(�ҥH�@�w�n���)

*�|���M�ϭq�����
[]�����\��
[]������T���

===============

Rooms(�|�����������ơA���w�g�i�H����쳡����ơA�P�w���e������)

==============
**�s�W��x**
�w�κ��F���s�W�����e��
*�|��:70%
*�q��:�|���s�@
*�Ы�:�|���s�@
*�̷s����

==============
����
[V]����̷s���U�e�Q���|��


=============
**
�|���޲z(80%)
[V]�|���d��
	add MemberRepoitory ��I�d�ߥ\��
	add ViewModels/MemberVm.cs 
	add DropdownListService.cs - �U�Կ��
	�j�Y��
	�����\��(�ϥήM��)

[]�s�譶���e�ݭ�����z

=============
[working on] �̷s������x������z
			�̷s�����e�x��z

=============
**����i�׬�����60%

==============